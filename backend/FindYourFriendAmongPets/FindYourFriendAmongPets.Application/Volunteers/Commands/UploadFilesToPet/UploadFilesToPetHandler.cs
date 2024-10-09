using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Application.Abstraction;
using FindYourFriendAmongPets.Application.Database;
using FindYourFriendAmongPets.Application.Extensions;
using FindYourFriendAmongPets.Application.Files;
using FindYourFriendAmongPets.Application.Messaging;
using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Shared;
using FindYourFriendAmongPets.Core.Shared.ValueObject;
using FluentValidation;
using Microsoft.Extensions.Logging;
using FileInfo = FindYourFriendAmongPets.Application.Files.FileInfo;

namespace FindYourFriendAmongPets.Application.Volunteers.Commands.UploadFilesToPet;

public class UploadFilesToPetHandler: ICommandHandler<Guid, UploadFilesToPetCommand>
{
    private const string BUCKET_NAME = "files";

    private readonly IFileProvider _fileProvider;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UploadFilesToPetCommand> _validator;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
    private readonly ILogger<UploadFilesToPetHandler> _logger;

    public UploadFilesToPetHandler(
        IFileProvider fileProvider,
        IVolunteerRepository volunteerRepository,
        IUnitOfWork unitOfWork,
        IValidator<UploadFilesToPetCommand> validator,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue,
        ILogger<UploadFilesToPetHandler> logger)
    {
        _fileProvider = fileProvider;
        _volunteerRepository = volunteerRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _messageQueue = messageQueue;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UploadFilesToPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }

        var volunteerResult = await _volunteerRepository
            .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);

        if (volunteerResult.IsFailure)
        {
            return volunteerResult.Error.ToErrorList();
        }

        var petId = PetId.Create(command.PetId);

        var petResult = volunteerResult.Value.GetPetById(petId);

        if (petResult.IsFailure)
        {
            return petResult.Error.ToErrorList();
        }

        List<FileData> filesData = [];
        foreach (var file in command.Files)
        {
            var extension = Path.GetExtension(file.FileName);

            var filePath = FilePath.Create(Guid.NewGuid(), extension);
            if (filePath.IsFailure)
                return filePath.Error.ToErrorList();

            var fileData = new FileData(file.Content, new FileInfo(filePath.Value, BUCKET_NAME));

            filesData.Add(fileData);
        }

        var filePathsResult = await _fileProvider.UploadFiles(filesData, cancellationToken);
        if (filePathsResult.IsFailure)
        {
            await _messageQueue.WriteAsync(filesData.Select(f => f.Info), cancellationToken);
            
            return filePathsResult.Error.ToErrorList();
        }

        var petFiles = filePathsResult.Value.Select(f => new PetFile(f));

        petResult.Value.UpdateFiles(new ValueObjectList<PetFile>(petFiles));

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Success uploaded files to pet - {id}", petResult.Value.Id.Value);

        return petResult.Value.Id.Value;
    }
}