using PetFriend.Core.Dtos;
using PetFriend.Volunteers.Application.Commands.AddPet;
using PetFriend.Volunteers.Domain.Enums;

namespace PetFriend.Volunteers.Presentation.Volunteers.Requests;

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