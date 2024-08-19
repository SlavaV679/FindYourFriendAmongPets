namespace FindYourFriendAmongPets.Core.Models;

public class Volunteer
{
    public Guid Id { get; private set; }

    public FullName FullName { get; private set; }

    public string Description { get; private set; }

    public int ExperienceInYears { get; private set; }

    public int CountPetsRealized { get; private set; }

    public int CountPetsLookingForHome { get; private set; }

    public int CountPetsHealing { get; private set; }

    public PhoneNumber PhoneNumber { get; private set; }

    public List<SocialNetwork> SocialNetworks { get; private set; } = [];

    public List<RequisiteForHelp> RequisitesForHelp { get; private set; } = [];

    public List<Pet> Pets { get; private set; } = [];
}