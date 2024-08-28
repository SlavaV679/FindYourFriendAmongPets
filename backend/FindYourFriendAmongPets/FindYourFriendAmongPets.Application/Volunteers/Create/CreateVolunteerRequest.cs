namespace FindYourFriendAmongPets.Application.Volunteers.Create;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    string PhoneNumber,
    int ExperienceInYears,
    IEnumerable<RequisiteForHelpDto>? RequisitesForHelpDto,
    IEnumerable<SocialNetworkDto>? SocialNetworksDto);