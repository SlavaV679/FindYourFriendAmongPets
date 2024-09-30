using FindYourFriendAmongPets.Application.Volunteers.Shared;

namespace FindYourFriendAmongPets.Application.Volunteers.Commands.UpdateMainInfo;

public record UpdateMainInfoCommand(
    Guid VolunteerId,
    FullNameDto FullName,
    string Description,
    string PhoneNumber,
    int ExperienceInYears);