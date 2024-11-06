using CSharpFunctionalExtensions;
using PetFriend.SharedKernel;
using PetFriend.SharedKernel.ValueObjects.Ids;

namespace PetFriend.Volunteers.Domain.SpeciesAggregate;

public class Species : SharedKernel.Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];

    //ef core
    private Species(SpeciesId id) : base(id)
    {
    }

    private Species(SpeciesId id, string name, List<Breed> breeds)
        : base(id)
    {
        Name = name;
        AddBreeds(breeds);
    }
    
    public string Name { get; private set; }

    public IReadOnlyList<Breed> Breeds => _breeds;

    public void AddBreeds(List<Breed> breeds) => _breeds.AddRange(breeds);

    public Result UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constants.MAX_LOW_TEXT_LENGHT)
        {
            return Result.Failure($"{name} cannot be null or have length more than {Constants.MAX_LOW_TEXT_LENGHT}");
        }

        Name = name;
        return Result.Success();
    }

    public static Result<Species> Create(SpeciesId id, string name, List<Breed> breeds)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constants.MAX_LOW_TEXT_LENGHT)
        {
            return Result.Failure<Species>($"{name} cannot be null or have length more than {Constants.MAX_LOW_TEXT_LENGHT}");
        }

        return Result.Success(new Species(id, name, breeds));
    }
}