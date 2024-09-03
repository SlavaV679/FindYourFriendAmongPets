using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Core.Abstractions;
using FindYourFriendAmongPets.Core.Shared;

namespace FindYourFriendAmongPets.Core.Models;

public class Volunteer : Shared.Entity<VolunteerId>, ISoftDeletable
{
    private bool _isDeleted = false;

    private readonly List<Pet> _pets = [];

    private readonly List<RequisiteForHelp> _requisitesForHelp = [];

    private readonly List<SocialNetwork> _socialNetworks = [];

    private Volunteer(VolunteerId id) : base(id)
    {
    }

    private Volunteer(VolunteerId id,
        FullName fullName,
        Description description,
        PhoneNumber phoneNumber,
        int experienceInYears,
        IEnumerable<RequisiteForHelp> requisitesForHelp,
        IEnumerable<SocialNetwork> socialNetworks)
        : base(id)
    {
        FullName = fullName;
        Description = description;
        ExperienceInYears = experienceInYears;
        PhoneNumber = phoneNumber;
        _requisitesForHelp = requisitesForHelp.ToList();
        _socialNetworks = socialNetworks.ToList();
    }

    public FullName FullName { get; private set; }

    public Description Description { get; private set; }

    public int ExperienceInYears { get; private set; }

    public PhoneNumber PhoneNumber { get; private set; }

    public IReadOnlyList<RequisiteForHelp> RequisitesForHelp => _requisitesForHelp;

    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;

    public IReadOnlyList<Pet> Pets => _pets;

    public static Result<Volunteer, Error> Create(VolunteerId id,
        FullName fullName,
        Description description,
        PhoneNumber phoneNumber,
        IEnumerable<RequisiteForHelp> requisitesForHelp,
        IEnumerable<SocialNetwork> socialNetworks,
        int experienceInYears = 0,
        int countPetsRealized = 0,
        int countPetsLookingForHome = 0,
        int countPetsHealing = 0)
    {
        if (experienceInYears < 0 || experienceInYears > Constants.MAX_YEARS_FOR_PERSON)
            return Errors.General.ValueIsInvalid(nameof(ExperienceInYears),
                $"{nameof(ExperienceInYears)} can not be less '0' ro bigger then '{Constants.MAX_YEARS_FOR_PERSON}'");

        return new Volunteer(id,
            fullName,
            description,
            phoneNumber,
            experienceInYears,
            requisitesForHelp,
            socialNetworks);
    }

    public int RealizedPetsCount() => _pets.Count(x => x.HelpStatus == Status.FoundHome);

    public int LookingForHomePetsCount() => _pets.Count(x => x.HelpStatus == Status.LookingForHome);

    public int HealingPetsCount() => _pets.Count(x => x.HelpStatus == Status.NeedsHelp);

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

    public override string ToString()
    {
        return $"{FullName.FirstName} {FullName.LastName} {FullName.Patronymic}";
    }

    public void Delete()
    {
        if (_isDeleted == false)
            _isDeleted = true;
    }

    public void Restore()
    {
        if (_isDeleted)
            _isDeleted = false;
    }
}