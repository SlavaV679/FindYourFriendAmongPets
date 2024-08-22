using FindYourFriendAmongPets.Core.Shared;

namespace FindYourFriendAmongPets.Core.Models;

public class Volunteer : Entity<VolunteerId>
{
    private Volunteer(VolunteerId id) : base(id)
    {
    }

    public Volunteer(VolunteerId id,
        FullName fullName,
        string description,
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