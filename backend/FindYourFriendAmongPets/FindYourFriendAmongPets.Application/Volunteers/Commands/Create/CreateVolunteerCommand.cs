using FindYourFriendAmongPets.Application.Volunteers.Shared;

namespace FindYourFriendAmongPets.Application.Volunteers.Commands.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Description,
    string PhoneNumber,
    int ExperienceInYears,
    IEnumerable<RequisiteForHelpDto>? RequisitesForHelpDto,
    IEnumerable<SocialNetworkDto>? SocialNetworksDto);