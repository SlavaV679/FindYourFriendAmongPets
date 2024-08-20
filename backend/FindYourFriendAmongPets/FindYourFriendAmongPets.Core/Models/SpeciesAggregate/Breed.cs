using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Core.Shared;

namespace FindYourFriendAmongPets.Core.Models.SpeciesAggregate;

public class Breed : Shared.Entity<BreedId>
{
    private Breed(BreedId id) : base(id)
    {
    }

    private Breed(BreedId id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public Result UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constants.MAX_LOW_TEXT_LENGHT)
            return Result.Failure($"{nameof(name)} cannot be null or length more than {Constants.MAX_LOW_TEXT_LENGHT}");

        Name = name;
        return Result.Success();
    }

    public static Result<Breed> Create(BreedId id, string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constants.MAX_LOW_TEXT_LENGHT)
            return Result.Failure<Breed>($"{nameof(name)} cannot be null or length more than {Constants.MAX_LOW_TEXT_LENGHT}");

        var breed = new Breed(id, name);

        return Result.Success(breed);
    }
}