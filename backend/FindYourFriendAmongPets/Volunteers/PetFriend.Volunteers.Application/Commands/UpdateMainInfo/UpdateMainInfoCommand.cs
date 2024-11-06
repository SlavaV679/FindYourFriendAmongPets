using PetFriend.Core.Abstractions;
using PetFriend.Core.Dtos;

namespace PetFriend.Volunteers.Application.Commands.UpdateMainInfo;

public record UpdateMainInfoCommand(
    Guid VolunteerId,
    FullNameDto FullName,
    string Description,
    string PhoneNumber,
    int ExperienceInYears): ICommand;