namespace FindYourFriendAmongPets.Application.Volunteers.Create;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    int ExperienceInYears,
    int CountPetsRealized,
    int CountPetsLookingForHome,
    int CountPetsHealing,
    string PhoneNumber,
    IEnumerable<RequisiteForHelpDto>? RequisitesForHelpDto,
    IEnumerable<SocialNetworkDto>? SocialNetworksDto);