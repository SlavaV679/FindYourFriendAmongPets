using PetFriend.Core.Dtos;
using PetFriend.Volunteers.Application.Commands.Create;

namespace PetFriend.Volunteers.Presentation.Volunteers.Requests;

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