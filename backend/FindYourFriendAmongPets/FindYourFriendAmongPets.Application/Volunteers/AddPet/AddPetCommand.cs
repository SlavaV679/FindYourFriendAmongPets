using FindYourFriendAmongPets.Core.Models;

namespace FindYourFriendAmongPets.Application.Volunteers.AddPet;

public record AddPetCommand(
    Guid VolunnteerId,
    string Name,
    PetSpeciesDto PetSpecies,
    string Description,
    string Color,
    string HealthInfo,
    AddressDto Address,
    double Weight,
    double Height,
    string OwnersPhoneNumber,
    bool IsNeutered,
    DateTime DateOfBirth,
    bool IsVaccinated,
    Status HelpStatus,
    //PetRequisiteDetails RequisiteDetails,
    IEnumerable<FileDto> Files);