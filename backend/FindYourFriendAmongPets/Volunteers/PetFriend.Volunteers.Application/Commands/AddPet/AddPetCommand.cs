using PetFriend.Core.Abstractions;
using PetFriend.Core.Dtos;
using PetFriend.Volunteers.Domain.Enums;

namespace PetFriend.Volunteers.Application.Commands.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
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
    ): ICommand;