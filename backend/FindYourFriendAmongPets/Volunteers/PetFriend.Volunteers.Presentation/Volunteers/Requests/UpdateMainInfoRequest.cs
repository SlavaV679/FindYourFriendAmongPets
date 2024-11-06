using PetFriend.Core.Dtos;
using PetFriend.Volunteers.Application.Commands.UpdateMainInfo;

namespace PetFriend.Volunteers.Presentation.Volunteers.Requests;

public record UpdateMainInfoRequest(
    FullNameDto FullName,
    string Description,
    string PhoneNumber,
    int ExperienceInYears)
{
    public UpdateMainInfoCommand ToCommand(Guid volunteerId) =>
        new(
            volunteerId,
            FullName,
            Description,
            PhoneNumber,
            ExperienceInYears);
}