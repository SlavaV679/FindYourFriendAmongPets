using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.UnitTests;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PetFriend.Core.Dtos;
using PetFriend.Core.Messaging;
using PetFriend.Core.Providers;
using PetFriend.SharedKernel;
using PetFriend.SharedKernel.ValueObjects;
using PetFriend.SharedKernel.ValueObjects.Ids;
using PetFriend.Volunteers.Application.Commands.UploadFilesToPet;
using PetFriend.Volunteers.Application.Database;
using PetFriend.Volunteers.Domain;
using FileInfo = PetFriend.Core.Providers.FileInfo;

namespace FindYourFriendAmongPets.Application.UnitTests;

public class UploadFilesToPetTests
{
    private readonly Mock<IFileProvider> _fileProviderMock = new();
    private readonly Mock<IVolunteerRepository> _volunteerRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IValidator<UploadFilesToPetCommand>> _validatorMock = new();
    private readonly Mock<IMessageQueue<IEnumerable<FileInfo>>> _messageQueueMock = new();

    [Fact]
    public async Task Handle_Should_Upload_Files_To_Pet()
    {
        // arrange
        var token = new CancellationTokenSource().Token;
        var stream = new MemoryStream();
        var filePath = FilePath.Create("test.jpg").Value;
        List<FilePath> filePathList = [filePath, filePath];
        var bucketName = "bucketName";
        var fileData = new FileData(stream, new FileInfo(filePath, bucketName));
        List<FileData> fileDataList = [fileData, fileData];
        var volunteer = BaseUnitTest.CreateVolunteer();
        var petId = PetId.NewPetId();
        var pet = BaseUnitTest.CreatePet(petId);
        volunteer.AddPet(pet);
        var fileNameDto = "fileNameDto.jpg";
        var uploadFileDto = new UploadFileDto(stream, fileNameDto);
        List<UploadFileDto> uploadFileDtoList = [uploadFileDto, uploadFileDto];
        var uploadFilesToPetCommand = new UploadFilesToPetCommand(volunteer.Id.Value, petId.Value, uploadFileDtoList);

        var fileProviderMock = new Mock<IFileProvider>();
        fileProviderMock.Setup(f => f.UploadFiles(It.IsAny<List<FileData>>(), token))
            .ReturnsAsync(() => filePathList);

        var volunteerRepositoryMock = new Mock<IVolunteerRepository>();
        volunteerRepositoryMock.Setup(r => r.GetById(It.IsAny<VolunteerId>(), token))
            .ReturnsAsync(Result.Success<Volunteer, Error>(volunteer));

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.SaveChanges(token))
            ; //.Returns(Task.CompletedTask);// тест вроходит в обоих случаях с этой строкой и без, но интерессно значть зачем она нужна?

        var validatorMock = new Mock<IValidator<UploadFilesToPetCommand>>();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UploadFilesToPetCommand>(), token))
            .ReturnsAsync(new ValidationResult());

        _messageQueueMock.Setup(m => m.WriteAsync(It.IsAny<IEnumerable<FileInfo>>(), token))
            .Returns(Task.CompletedTask);
        
        var loggerMock = new Mock<ILogger<UploadFilesToPetHandler>>();

        var uploadFilesToPetHandler = new UploadFilesToPetHandler(
            fileProviderMock.Object,
            volunteerRepositoryMock.Object,
            unitOfWorkMock.Object,
            validatorMock.Object,
            _messageQueueMock.Object,
            loggerMock.Object);

        // act
        var result = await uploadFilesToPetHandler.Handle(uploadFilesToPetCommand, token);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(petId.Value);
        volunteer.Pets[0].PetFiles.Count.Should().Be(2);
        volunteer.Pets[0].PetFiles.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_Should_Return_Error_When_Upload_Files()
    {
        // arrange
        const string errorCode = "fail.fail.upload.mock";
        const string errorMessage = "Return mock error when upload files";

        var volunteer = BaseUnitTest.CreateVolunteer();
        var petId = PetId.NewPetId();
        var pet = BaseUnitTest.CreatePet(petId);
        volunteer.AddPet(pet);

        var token = new CancellationTokenSource().Token;
        var stream = new MemoryStream();
        const string fileNameDto = "fileNameDto.jpg";
        var uploadFileDto = new UploadFileDto(stream, fileNameDto);
        List<UploadFileDto> uploadFileDtoList = [uploadFileDto, uploadFileDto];
        var uploadFilesToPetCommand = new UploadFilesToPetCommand(volunteer.Id.Value, petId.Value, uploadFileDtoList);

        _fileProviderMock.Setup(f => f.UploadFiles(It.IsAny<List<FileData>>(), token))
            .ReturnsAsync(Result.Failure<IReadOnlyList<FilePath>, Error>(Error.Validation(errorCode, errorMessage)));

        _volunteerRepositoryMock.Setup(r => r.GetById(It.IsAny<VolunteerId>(), token))
            .ReturnsAsync(Result.Success<Volunteer, Error>(volunteer));

        _unitOfWorkMock.Setup(u => u.SaveChanges(token))
            ; //.Returns(Task.CompletedTask);// тест вроходит в обоих случаях с этой строкой и без, но интерессно значть зачем она нужна?

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UploadFilesToPetCommand>(), token))
            .ReturnsAsync(new ValidationResult());

        _messageQueueMock.Setup(m => m.WriteAsync(It.IsAny<IEnumerable<FileInfo>>(), token))
            .Returns(Task.CompletedTask);

        var loggerMock = new Mock<ILogger<UploadFilesToPetHandler>>();

        var uploadFilesToPetHandler = new UploadFilesToPetHandler(
            _fileProviderMock.Object,
            _volunteerRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _validatorMock.Object,
            _messageQueueMock.Object,
            loggerMock.Object);

        // act
        var result = await uploadFilesToPetHandler.Handle(uploadFilesToPetCommand, token);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Error.Count().Should().Be(1);
        result.Error.First().Code.Should().Be(errorCode);
        result.Error.First().Message.Should().Be(errorMessage);
    }
    
    [Fact]
    public async Task Handle_Should_Return_Error_When_Try_Create_File_With_Unsupported_Format()
    {
        // arrange
        //TODO можно ли как то volunteer создавать соверешенно любого
        //типа It.IsAny<> и передавать его вкачестве ответа мока?
        //.returns(It.IsAny<>()). На данный момент не получается , т.к.
        //в ответе успешный Result  с пустым volunteer.
        var volunteer = BaseUnitTest.CreateVolunteer();
        var petId = PetId.NewPetId();
        var pet = BaseUnitTest.CreatePet(petId);
        volunteer.AddPet(pet);

        var token = new CancellationTokenSource().Token;
        var stream = new MemoryStream();
        const string fileNameDto = "fileNameDto.mp3";
        var uploadFileDto = new UploadFileDto(stream, fileNameDto);
        List<UploadFileDto> uploadFileDtoList = [uploadFileDto, uploadFileDto];
        var uploadFilesToPetCommand = new UploadFilesToPetCommand(volunteer.Id.Value, petId.Value, uploadFileDtoList);

        _fileProviderMock.Setup(f => f.UploadFiles(It.IsAny<List<FileData>>(), token))
            .ReturnsAsync(It.IsAny<Result<IReadOnlyList<FilePath>, Error>>());

        _volunteerRepositoryMock.Setup(r => r.GetById(It.IsAny<VolunteerId>(), token))
            .ReturnsAsync(Result.Success<Volunteer, Error>(volunteer));

        _unitOfWorkMock.Setup(u => u.SaveChanges(token))
            ; //.Returns(Task.CompletedTask);// тест вроходит в обоих случаях с этой строкой и без, но интерессно значть зачем она нужна?

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UploadFilesToPetCommand>(), token))
            .ReturnsAsync(new ValidationResult());
        
        _messageQueueMock.Setup(m => m.WriteAsync(It.IsAny<IEnumerable<FileInfo>>(), token))
            .Returns(Task.CompletedTask);
        
        var loggerMock = new Mock<ILogger<UploadFilesToPetHandler>>();

        var uploadFilesToPetHandler = new UploadFilesToPetHandler(
            _fileProviderMock.Object,
            _volunteerRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _validatorMock.Object,
            _messageQueueMock.Object,
            loggerMock.Object);

        // act
        var result = await uploadFilesToPetHandler.Handle(uploadFilesToPetCommand, token);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Error.Count().Should().Be(1);
        result.Error.First().Code.Should().Be(Errors.General.VALIDATION_ERROR_CODE);
    }

    [Fact]
    public async Task Handle_Should_Throw_DbUpdateException_When_SaveChanges()
    {
        // arrange
        var volunteer = BaseUnitTest.CreateVolunteer();
        var petId = PetId.NewPetId();
        var pet = BaseUnitTest.CreatePet(petId);
        volunteer.AddPet(pet);
        var filePath = FilePath.Create("test.jpg").Value;
        List<FilePath> filePathList = [filePath, filePath];

        var token = new CancellationTokenSource().Token;
        var stream = new MemoryStream();
        const string fileNameDto = "fileNameDto.jpg";
        var uploadFileDto = new UploadFileDto(stream, fileNameDto);
        List<UploadFileDto> uploadFileDtoList = [uploadFileDto, uploadFileDto];
        var uploadFilesToPetCommand = new UploadFilesToPetCommand(volunteer.Id.Value, petId.Value, uploadFileDtoList);

        _fileProviderMock.Setup(f => f.UploadFiles(It.IsAny<List<FileData>>(), token))
            .ReturnsAsync(() => filePathList);

        _volunteerRepositoryMock.Setup(r => r.GetById(It.IsAny<VolunteerId>(), token))
            .ReturnsAsync(Result.Success<Volunteer, Error>(volunteer));

        _unitOfWorkMock.Setup(u => u.SaveChanges(token))
            .Throws(new DbUpdateException());
        
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UploadFilesToPetCommand>(), token))
            .ReturnsAsync(new ValidationResult());

        _messageQueueMock.Setup(m => m.WriteAsync(It.IsAny<IEnumerable<FileInfo>>(), token))
            .Returns(Task.CompletedTask);
        
        var loggerMock = new Mock<ILogger<UploadFilesToPetHandler>>();

        var uploadFilesToPetHandler = new UploadFilesToPetHandler(
            _fileProviderMock.Object,
            _volunteerRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _validatorMock.Object,
            _messageQueueMock.Object,
            loggerMock.Object);
        
        // act
        Func<Task> act = () => uploadFilesToPetHandler.Handle(uploadFilesToPetCommand, token);

        //Assert
        await Assert.ThrowsAsync<DbUpdateException>(act);
    }
}