using FindYourFriendAmongPets.Application.Volunteers.Commands.UpdateMainInfo;
using FindYourFriendAmongPets.Application.Volunteers.Shared;

namespace FindYourFriendAmongPets.API.Controllers.Requests;

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