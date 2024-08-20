using CSharpFunctionalExtensions;

namespace FindYourFriendAmongPets.Core.Models;

public record PetSpecies
{
    private PetSpecies(Guid speciesId, Guid breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }

    public Guid SpeciesId { get; }
    public Guid BreedId { get; }

    public static Result<PetSpecies> Create(Guid speciesId, Guid breedId)
    {
        var speciesBreed = new PetSpecies(speciesId, breedId);
        return Result.Success(speciesBreed);
    }
}