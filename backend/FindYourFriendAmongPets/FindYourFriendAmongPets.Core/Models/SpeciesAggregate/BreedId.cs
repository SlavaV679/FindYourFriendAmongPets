namespace FindYourFriendAmongPets.Core.Models.SpeciesAggregate;

public record BreedId
{
    protected BreedId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static BreedId Create(Guid value) => new(value);
    
    public static BreedId NewBreedId() => new(Guid.NewGuid());
    
    public static BreedId Empty() => new(Guid.Empty);
}