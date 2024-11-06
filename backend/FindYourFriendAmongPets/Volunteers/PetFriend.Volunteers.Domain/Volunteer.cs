using CSharpFunctionalExtensions;
using PetFriend.SharedKernel;
using PetFriend.SharedKernel.Abstractions;
using PetFriend.SharedKernel.ValueObjects;
using PetFriend.SharedKernel.ValueObjects.Ids;
using PetFriend.Volunteers.Domain.Entities;
using PetFriend.Volunteers.Domain.Enums;
using PetFriend.Volunteers.Domain.ValueObject;

namespace PetFriend.Volunteers.Domain;

public class Volunteer : CSharpFunctionalExtensions.Entity<VolunteerId>, ISoftDeletable
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

    public void UpdateMainInfo(
        FullName fullName,
        Description description,
        PhoneNumber phoneNumber,
        int experienceInYears = 0)
    {
        FullName = fullName;
        Description = description;
        PhoneNumber = phoneNumber;
        ExperienceInYears = experienceInYears;
    }

    public void Delete()
    {
        _isDeleted = true;

        foreach (var pet in _pets)
        {
            pet.Delete();
        }
    }

    public void Restore()
    {
        _isDeleted = false;

        foreach (var pet in _pets)
        {
            pet.Restore();
        }
    }

    public UnitResult<Error> AddPet(Pet pet)
    {
        // валидация + логика
        var positionResult = Position.Create(_pets.Count + 1);
        if (positionResult.IsFailure)
            return positionResult.Error;

        pet.SetPosition(positionResult.Value);

        _pets.Add(pet);
        return Result.Success<Error>();
    }

    public Result<Pet, Error> GetPetById(PetId petId)
    {
        var pet = _pets.FirstOrDefault(i => i.Id.Value == petId.Value);
        if (pet is null)
            return Errors.General.NotFound(petId.Value);

        return pet;
    }

    public UnitResult<Error> MovePet(Pet pet, Position newPosition)
    {
        var currentPosition = pet.Position;

        if (currentPosition == newPosition || _pets.Count == 1)
            return Result.Success<Error>();

        var adjustedPosition = AdjustNewPositionIfOutOfRange(newPosition);
        if (adjustedPosition.IsFailure)
            return adjustedPosition.Error;

        newPosition = adjustedPosition.Value;

        var moveResult = MovePetsBetweenPositions(currentPosition, newPosition);
        if (moveResult.IsFailure)
            return moveResult.Error;

        pet.Move(newPosition);

        return Result.Success<Error>();
    }

    private UnitResult<Error> MovePetsBetweenPositions(Position currentPosition, Position newPosition)
    {
        if (currentPosition > newPosition)
        {
            var petsToMove = _pets
                .Where(i => i.Position >= newPosition && i.Position < currentPosition);

            foreach (var petToMove in petsToMove)
            {
                var result = petToMove.MoveForward();
                if (result.IsFailure)
                {
                    return result.Error;
                }
            }
        }
        else if (currentPosition < newPosition)
        {
            var petsToMove = _pets
                .Where(i => i.Position > currentPosition && i.Position <= newPosition);

            foreach (var petToMove in petsToMove)
            {
                var result = petToMove.MoveBack();
                if (result.IsFailure)
                {
                    return result.Error;
                }
            }
        }

        return Result.Success<Error>();
    }

    private Result<Position, Error> AdjustNewPositionIfOutOfRange(Position newPosition)
    {
        if (newPosition.Value <= _pets.Count)
            return newPosition;

        var lastPosition = Position.Create(_pets.Count);
        if (lastPosition.IsFailure)
            return lastPosition.Error;

        return lastPosition.Value;
    }
}