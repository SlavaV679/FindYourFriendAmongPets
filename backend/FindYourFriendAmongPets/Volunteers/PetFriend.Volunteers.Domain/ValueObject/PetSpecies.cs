using CSharpFunctionalExtensions;
using PetFriend.SharedKernel;
using PetFriend.SharedKernel.ValueObjects.Ids;

namespace PetFriend.Volunteers.Domain.ValueObject;

public record PetSpecies
{
    private PetSpecies()
    {
    }
    
    private PetSpecies(SpeciesId speciesId, Guid breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }

    public SpeciesId SpeciesId { get; }
    
    public Guid BreedId { get; }

    public static Result<PetSpecies, Error> Create(SpeciesId speciesId, Guid breedId)
    {
        var speciesBreed = new PetSpecies(speciesId, breedId);
        
        return speciesBreed;
    }
}