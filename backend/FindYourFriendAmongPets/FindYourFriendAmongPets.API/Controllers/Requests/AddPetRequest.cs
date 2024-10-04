using FindYourFriendAmongPets.Application.Volunteers.Commands.AddPet;
using FindYourFriendAmongPets.Application.Volunteers.Shared;
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
    Status HelpStatus
    //PetRequisiteDetails RequisiteDetails,
)
{
    public AddPetCommand ToCommand(Guid volunteerId) =>
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
            HelpStatus);
}