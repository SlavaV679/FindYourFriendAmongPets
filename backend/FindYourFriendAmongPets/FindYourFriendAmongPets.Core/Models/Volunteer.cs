using FindYourFriendAmongPets.Core.Shared;

namespace FindYourFriendAmongPets.Core.Models;

public class Volunteer : Entity<VolunteerId>
{
    private readonly List<RequisiteForHelp> _requisitesForHelp = [];

    private readonly List<SocialNetwork> _socialNetworks = [];

    private Volunteer(VolunteerId id) : base(id)
    {
    }

    public Volunteer(VolunteerId id,
        FullName fullName,
        Description description,
        int experienceInYears,
        int countPetsRealized,
        int countPetsLookingForHome,
        int countPetsHealing,
        PhoneNumber phoneNumber)
        : base(id)
    {
        FullName = fullName;
        Description = description;
        ExperienceInYears = experienceInYears;
        CountPetsRealized = countPetsRealized;
        CountPetsLookingForHome = countPetsLookingForHome;
        CountPetsHealing = countPetsHealing;
        PhoneNumber = phoneNumber;
    }

    public FullName FullName { get; private set; }

    public Description Description { get; private set; }

    public int ExperienceInYears { get; private set; }

    public int CountPetsRealized { get; private set; }

    public int CountPetsLookingForHome { get; private set; }

    public int CountPetsHealing { get; private set; }

    public PhoneNumber PhoneNumber { get; private set; }

    public IReadOnlyList<RequisiteForHelp> RequisitesForHelp => _requisitesForHelp;

    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;

    public List<Pet> Pets { get; private set; } = [];

    public void AddRequisitesForHelp(IEnumerable<RequisiteForHelp>? requisitesForHelp)
    {
        if (requisitesForHelp != null)
            _requisitesForHelp.AddRange(requisitesForHelp);
    }

    public void AddSocialNetwork(IEnumerable<SocialNetwork>? socialNetworks)
    {
        if (socialNetworks != null)
            _socialNetworks.AddRange(socialNetworks);
    }
}