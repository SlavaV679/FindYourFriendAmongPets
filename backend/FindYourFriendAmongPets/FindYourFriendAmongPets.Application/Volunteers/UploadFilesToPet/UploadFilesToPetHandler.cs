using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Application.Database;
using FindYourFriendAmongPets.Application.Extensions;
using FindYourFriendAmongPets.Application.FileProvider;
using FindYourFriendAmongPets.Application.Providers;
using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Shared;
using FindYourFriendAmongPets.Core.Shared.ValueObject;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace FindYourFriendAmongPets.Application.Volunteers.UploadFilesToPet;

public class UploadFilesToPetHandler
{
        private const string BUCKET_NAME = "files";

    private readonly IFileProvider _fileProvider;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UploadFilesToPetCommand> _validator;
    private readonly ILogger<UploadFilesToPetHandler> _logger;

    public UploadFilesToPetHandler(
        IFileProvider fileProvider,
        IVolunteerRepository volunteerRepository,
        IUnitOfWork unitOfWork,
        IValidator<UploadFilesToPetCommand> validator,
        ILogger<UploadFilesToPetHandler> logger)
    {
        _fileProvider = fileProvider;
        _volunteerRepository = volunteerRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
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
        if(petResult.IsFailure)
            return petResult.Error.ToErrorList();

        List<FileData> filesData = [];
        foreach (var file in command.Files)
        {
            var extension = Path.GetExtension(file.FileName);

            var filePath = FilePath.Create(Guid.NewGuid(), extension);
            if (filePath.IsFailure)
                return filePath.Error.ToErrorList();

            var fileData = new FileData(file.Content, filePath.Value, BUCKET_NAME);

            filesData.Add(fileData);
        }

        var filePathsResult = await _fileProvider.UploadFiles(filesData, cancellationToken);
        if (filePathsResult.IsFailure)
            return filePathsResult.Error.ToErrorList();

        var petFiles = filePathsResult.Value
            .Select(f => new PetPhoto(f));

        petResult.Value.UpdateFiles(new ValueObjectList<PetPhoto>(petFiles));

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Success uploaded files to pet - {id}", petResult.Value.Id.Value);

        return petResult.Value.Id.Value;
    }
}