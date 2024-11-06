using PetFriend.Core.Abstractions;
using PetFriend.Core.Dtos;

namespace PetFriend.Volunteers.Application.Commands.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Description,
    string PhoneNumber,
    int ExperienceInYears,
    IEnumerable<RequisiteForHelpDto>? RequisitesForHelpDto,
    IEnumerable<SocialNetworkDto>? SocialNetworksDto): ICommand;