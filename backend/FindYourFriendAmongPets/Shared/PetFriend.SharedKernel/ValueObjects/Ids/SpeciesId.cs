namespace PetFriend.SharedKernel.ValueObjects.Ids;

public record SpeciesId
{
    protected SpeciesId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static SpeciesId Create(Guid value) => new(value);

    public static SpeciesId NewSpeciesId() => new(Guid.NewGuid());

    public static SpeciesId Empty() => new(Guid.Empty);
}