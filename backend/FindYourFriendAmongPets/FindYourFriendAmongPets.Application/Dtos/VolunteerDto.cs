using FindYourFriendAmongPets.Application.Volunteers.Shared;

namespace FindYourFriendAmongPets.Application.Dtos;

public class VolunteerDto
{
    public Guid Id { get; init; }
    // public FullNameDto FullName { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; }
    public string? Patronymic { get; init; }
    public string Description { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public int ExperienceInYears { get; init; }
    
    // public PetDto[] Pets { get; init; } = [];
    //public IEnumerable<RequisiteForHelpDto>? RequisitesForHelpDto { get; init; }
    //public IEnumerable<SocialNetworkDto>? SocialNetworksDto { get; init; }
}