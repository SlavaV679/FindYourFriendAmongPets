using FindYourFriendAmongPets.Application.Volunteers.Create;
using FindYourFriendAmongPets.Application.Volunteers.Shared;

namespace FindYourFriendAmongPets.API.Controllers.Requests;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    string PhoneNumber,
    int ExperienceInYears,
    IEnumerable<RequisiteForHelpDto>? RequisitesForHelpDto,
    IEnumerable<SocialNetworkDto>? SocialNetworksDto)
{
    public CreateVolunteerCommand ToCommand() =>
        new(
            FullName,
            Description,
            PhoneNumber,
            ExperienceInYears,
            RequisitesForHelpDto,
            SocialNetworksDto);
}