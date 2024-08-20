using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Core.Models.SpeciesAggregate;

namespace FindYourFriendAmongPets.Core.Models;

public record PetSpecies
{
    private PetSpecies(SpeciesId speciesId, Guid breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }

    public SpeciesId SpeciesId { get; }
    
    public Guid BreedId { get; }

    public static Result<PetSpecies> Create(SpeciesId speciesId, Guid breedId)
    {
        var speciesBreed = new PetSpecies(speciesId, breedId);
        return Result.Success(speciesBreed);
    }
}