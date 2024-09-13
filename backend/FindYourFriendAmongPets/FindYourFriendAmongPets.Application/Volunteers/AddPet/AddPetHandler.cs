using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Application.Database;
using FindYourFriendAmongPets.Application.Extensions;
using FindYourFriendAmongPets.Application.FileProvider;
using FindYourFriendAmongPets.Application.Providers;
using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Models.SpeciesAggregate;
using FindYourFriendAmongPets.Core.Shared;
using FindYourFriendAmongPets.Core.Shared.ValueObject;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace FindYourFriendAmongPets.Application.Volunteers.AddPet;

public class AddPetHandler
{
    private const string BUCKET_NAME = "files";

    private readonly IFileProvider _fileProvider;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddPetCommand> _validator;
    private readonly ILogger<AddPetHandler> _logger;

    public AddPetHandler(
        IFileProvider fileProvider,
        IVolunteerRepository volunteerRepository,
        IUnitOfWork unitOfWork,
        IValidator<AddPetCommand> validator,
        ILogger<AddPetHandler> logger)
    {
        _fileProvider = fileProvider;
        _volunteerRepository = volunteerRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }
        
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        try
        {
            var volunteerResult = await _volunteerRepository
                .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);

            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var petId = PetId.NewPetId();
            var description = Description.Create(command.Description).Value;
            var petSpecies = PetSpecies
                .Create(SpeciesId.Create(command.PetSpecies.SpeciesId), command.PetSpecies.BreedId)
                .Value;
            var address = Address.Create(
                command.Address.City,
                command.Address.Street,
                command.Address.Building,
                command.Address.Description,
                command.Address.Country).Value;

            List<FileData> filesData = [];
            foreach (var file in command.FileCommands)
            {
                var extension = Path.GetExtension(file.FileName);

                var filePath = FilePath.Create(Guid.NewGuid(), extension);
                if (filePath.IsFailure)
                    return filePath.Error.ToErrorList();

                var fileContent = new FileData(file.Content, filePath.Value, BUCKET_NAME);

                filesData.Add(fileContent);
            }

            var filePaths = filesData
                .Select(f => f.FilePath)
                .Select(f => FilePath.Create(f.Path).Value)
                .ToList();

            var petPhotos = filePaths.Select(f => new PetPhoto(f, false));

            var requisite = Requisite.Create("name of Requisite", "description");
            var requisiteList = new PetRequisiteDetails([requisite.Value]);

            var pet = Pet.Create(PetId.NewPetId(),
                command.Name,
                petSpecies,
                description,
                command.Color,
                command.HealthInfo,
                address,
                command.Weight,
                command.Height,
                PhoneNumber.Create(command.OwnersPhoneNumber).Value,
                command.IsNeutered,
                command.DateOfBirth.ToUniversalTime(),
                command.IsVaccinated,
                command.HelpStatus,
                requisiteList,
                new ValueObjectList<PetPhoto>(petPhotos));

            volunteerResult.Value.AddPet(pet.Value);

            await _unitOfWork.SaveChanges(cancellationToken);

            var uploadResult = await _fileProvider.UploadFiles(filesData, cancellationToken);

            if (uploadResult.IsFailure)
                return uploadResult.Error.ToErrorList();

            transaction.Commit();

            return pet.Value.Id.Value;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,"Can not add pet photo to volunteer - {id} in transaction", command.VolunteerId);

            transaction.Rollback();

            return Error.Failure("module.issue.failure",$"Can not add pet photo to volunteer  - {command.VolunteerId}")
                .ToErrorList();
        }
    }
}