using FindYourFriendAmongPets.Application.Volunteers.Shared;

namespace FindYourFriendAmongPets.Application.Volunteers.UpdateMainInfo;

public record UpdateMainInfoDto(
    FullNameDto FullName,
    string Description,
    string PhoneNumber,
    int ExperienceInYears);