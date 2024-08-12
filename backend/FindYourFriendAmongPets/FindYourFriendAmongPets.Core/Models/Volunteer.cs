namespace FindYourFriendAmongPets.Core.Models;

public class Volunteer
{
    public Guid Id { get; private set; }

    public string FullName { get; private set; }

    public string Descriptions { get; private set; }

    public int ExperienceInYears { get; private set; }

    public int CountPetsRealized { get; private set; }

    public int CountPetsLookingForHome { get; private set; }

    public int CountPetsHealing { get; private set; }

    public string PhoneNumber { get; private set; }

    public List<SocialMedia> SocialMedias { get; private set; } = [];

    public List<RequisiteForHelp> RequisiteForHelps { get; private set; } = [];

    public List<Pet> Pets { get; private set; } = [];
}