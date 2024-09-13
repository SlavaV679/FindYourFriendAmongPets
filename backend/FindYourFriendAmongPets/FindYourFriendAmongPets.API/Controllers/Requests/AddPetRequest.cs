using FindYourFriendAmongPets.Application.Volunteers.AddPet;
using FindYourFriendAmongPets.Core.Models;

namespace FindYourFriendAmongPets.API.Controllers.Requests;

public record AddPetRequest(
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
    IFormFileCollection Files)
{
    public AddPetCommand ToCommand(Guid volunteerId, IEnumerable<CreateFileCommand> fileContents) =>
        new(
            volunteerId,
            Name,
            PetSpecies,
            Description,
            Color,
            HealthInfo,
            Address,
            Weight,
            Height,
            OwnersPhoneNumber,
            IsNeutered,
            DateOfBirth,
            IsVaccinated,
            HelpStatus,
            fileContents);
}