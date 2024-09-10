using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Core.Abstractions;
using FindYourFriendAmongPets.Core.Shared;

namespace FindYourFriendAmongPets.Core.Models;

public class Pet : Shared.Entity<PetId>, ISoftDeletable
{
    private bool _isDeleted = false;

    private readonly List<PetPhoto> _petPhotos = [];

    private Pet(PetId id) : base(id)
    {
    }

    private Pet(PetId id,
        string name,
        PetSpecies petSpecies,
        Description description,
        string color,
        string healthInfo,
        Address address,
        double weight,
        double height,
        PhoneNumber ownersPhoneNumber,
        bool isNeutered,
        DateTime dateOfBirth,
        bool isVaccinated,
        Status helpStatus,
        DateTime dateCreated,
        PetRequisiteDetails requisiteDetails,
        PetPhotosList petPhotos)
        : base(id)
    {
        Name = name;
        PetSpecies = petSpecies;
        Description = description;
        Color = color;
        HealthInfo = healthInfo;
        Address = address;
        Weight = weight;
        Height = height;
        OwnersPhoneNumber = ownersPhoneNumber;
        IsNeutered = isNeutered;
        DateOfBirth = dateOfBirth;
        IsVaccinated = isVaccinated;
        HelpStatus = helpStatus;
        DateCreated = dateCreated;
        RequisiteDetails = requisiteDetails;
        PetPhotos = petPhotos;
    }

    public string Name { get; private set; }

    public PetSpecies PetSpecies { get; private set; }

    public Description Description { get; private set; }

    public string Color { get; private set; }

    public string HealthInfo { get; private set; }

    public Address Address { get; private set; }

    public double Weight { get; private set; }

    public double Height { get; private set; }

    public PhoneNumber OwnersPhoneNumber { get; private set; }

    public bool IsNeutered { get; private set; }

    public DateTime DateOfBirth { get; private set; }

    public bool IsVaccinated { get; private set; }

    public Status HelpStatus { get; private set; }

    public DateTime DateCreated { get; private set; }

    public PetPhotosList PetPhotos { get; private set; }

    public PetRequisiteDetails RequisiteDetails { get; private set; }

    public static Result<Pet, Error> Create(PetId id,
        string name,
        PetSpecies petSpecies,
        Description description,
        string color,
        string healthInfo,
        Address address,
        double weight,
        double height,
        PhoneNumber ownersPhoneNumber,
        bool isNeutered,
        DateTime dateOfBirth,
        bool isVaccinated,
        Status helpStatus,
        PetRequisiteDetails requisiteDetails,
        PetPhotosList petPhotos
    )
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constants.MAX_LOW_TEXT_LENGHT)
            return Errors.General.ValueIsInvalid(
                nameof(Name), $"{nameof(Name)} can not be empty or bigger then {Constants.MAX_LOW_TEXT_LENGHT}");

        if (string.IsNullOrWhiteSpace(color) || color.Length > Constants.MAX_LOW_TEXT_LENGHT)
            return Errors.General.ValueIsInvalid(
                nameof(Color), $"{nameof(Color)} can not be empty or bigger then {Constants.MAX_LOW_TEXT_LENGHT}");

        if (string.IsNullOrWhiteSpace(healthInfo) || healthInfo.Length > Constants.MAX_HIGH_TEXT_LENGHT)
            return Errors.General.ValueIsInvalid(
                nameof(HealthInfo), $"{nameof(HealthInfo)} can not be empty or bigger then {Constants.MAX_HIGH_TEXT_LENGHT}");

        return new Pet(id,
            name,
            petSpecies,
            description,
            color,
            healthInfo,
            address,
            weight,
            height,
            ownersPhoneNumber,
            isNeutered,
            dateOfBirth,
            isVaccinated,
            helpStatus,
            DateTime.Now.ToUniversalTime(),
            requisiteDetails,
            petPhotos);
    }

    public void AddPetPhotos(IEnumerable<PetPhoto> petPhotos)
    {
        _petPhotos.AddRange(petPhotos);
    }


    public void Delete()
    {
        if (_isDeleted == false)
            _isDeleted = true;
    }

    public void Restore()
    {
        if (_isDeleted)
            _isDeleted = false;
    }
}