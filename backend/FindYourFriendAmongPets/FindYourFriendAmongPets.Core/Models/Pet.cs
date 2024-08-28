using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Core.Shared;

namespace FindYourFriendAmongPets.Core.Models;

public class Pet : Shared.Entity<PetId>
{
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
        DateOnly dateOfBirth,
        bool isVaccinated,
        Status helpStatus,
        DateTime dateCreated,
        PetRequisiteDetails requisiteDetails)
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

    public DateOnly DateOfBirth { get; private set; }

    public bool IsVaccinated { get; private set; }

    public Status HelpStatus { get; private set; }

    public DateTime DateCreated { get; private set; }

    public IReadOnlyList<PetPhoto> PetPhotos => _petPhotos;

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
        DateOnly dateOfBirth,
        bool isVaccinated,
        Status helpStatus ,
        PetRequisiteDetails requisiteDetails
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
            DateTime.Now,
            requisiteDetails);
    }
    
    public void AddPetPhotos(IEnumerable<PetPhoto> petPhotos)
    {
            _petPhotos.AddRange(petPhotos);
    }
}