using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.UnitTests;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using PetFriend.Core.Dtos;
using PetFriend.SharedKernel;
using PetFriend.SharedKernel.ValueObjects.Ids;
using PetFriend.Volunteers.Application.Commands.AddPet;
using PetFriend.Volunteers.Application.Database;
using PetFriend.Volunteers.Domain;
using PetFriend.Volunteers.Domain.Enums;

namespace FindYourFriendAmongPets.Application.UnitTests;

public class AddPetHandlerTests
{
    private readonly Mock<IVolunteerRepository> _volunteerRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IValidator<AddPetCommand>> _validatorMock;

    public AddPetHandlerTests()
    {
        _volunteerRepositoryMock = new Mock<IVolunteerRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<AddPetCommand>>();
    }

    [Fact]
    public async Task Handle_Should_Add_Pet_To_Volunteer()
    {
        // arrange
        var token = new CancellationTokenSource().Token;
        var volunteer = BaseUnitTest.CreateVolunteer();

        var petSpeciesDto = new PetSpeciesDto(Guid.NewGuid(), Guid.NewGuid());
        var addressDto = new AddressDto("City", "Street");
        var addPetCommand = new AddPetCommand(
            volunteer.Id.Value, 
            $"Name of Pet",
            petSpeciesDto,
            "Description",
            "orange",
            "health info",
            addressDto,
            10,
            20,
            "+20240918101",
            true,
            DateTime.Now,
            true,
            Status.LookingForHome);
        
        _volunteerRepositoryMock.Setup(r => r.GetById(It.IsAny<VolunteerId>(), token))
            .ReturnsAsync(Result.Success<Volunteer, Error>(volunteer));
            //Интерессно можно ли здесь вернуть какого-нибудь любого Volunteer,
            //т.к. нам в принципе без разницы к какому volunteer добавлять pet?
            //например как нибудь так сделать: ReturnsAsync(new Volunteer());


        _unitOfWorkMock.Setup(u => u.SaveChanges(token));
        
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AddPetCommand>(), token))
            .ReturnsAsync(new ValidationResult());

        var loggerMock = new Mock<ILogger<AddPetHandler>>();

        var addPetHandler = new AddPetHandler(
            _volunteerRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _validatorMock.Object,
            loggerMock.Object);
        
        // act
        var result = await addPetHandler.Handle(addPetCommand, token);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        volunteer.Pets.Should().NotBeNullOrEmpty();
        volunteer.Pets.Should().HaveCount(1);
    }
}