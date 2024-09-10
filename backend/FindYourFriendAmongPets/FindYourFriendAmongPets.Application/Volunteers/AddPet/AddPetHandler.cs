using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Application.FileProvider;
using FindYourFriendAmongPets.Application.Providers;
using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Models.SpeciesAggregate;
using FindYourFriendAmongPets.Core.Shared;

namespace FindYourFriendAmongPets.Application.Volunteers.AddPet;

public class AddPetHandler
{
    private const string BUCKET_NAME = "files";

    private readonly IFileProvider _fileProvider;
    private readonly IVolunteerRepository _volunteerRepository;

    public AddPetHandler(IFileProvider fileProvider, IVolunteerRepository volunteerRepository)
    {
        _fileProvider = fileProvider;
        _volunteerRepository = volunteerRepository;
    }

    public async Task<Result<Guid, Error>> Handle(
        AddPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteerRepository
            .GetById(VolunteerId.Create(command.VolunnteerId), cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

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

        List<FileContent> fileContents = [];
        foreach (var file in command.Files)
        {
            var extension = Path.GetExtension(file.FileName);

            var filePath = FilePath.Create(Guid.NewGuid(), extension);
            if (filePath.IsFailure)
                return filePath.Error;

            var fileContent = new FileContent(
                file.Content, filePath.Value.Path);

            fileContents.Add(fileContent);
        }

        var fileData = new FileData(fileContents, BUCKET_NAME);

        var uploadResult = await _fileProvider
            .UploadFiles(fileData, cancellationToken);

        if (uploadResult.IsFailure)
            return uploadResult.Error;

        var filePaths = command.Files
            .Select(f => FilePath.Create(Guid.NewGuid(), f.FileName).Value);

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
            new PetPhotosList(petPhotos));

        volunteerResult.Value.AddPet(pet.Value);

        await _volunteerRepository.Save(volunteerResult.Value, cancellationToken);

        return pet.Value.Id.Value;
    }
}