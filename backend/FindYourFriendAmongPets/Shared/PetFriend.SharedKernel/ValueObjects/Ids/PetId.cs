namespace PetFriend.SharedKernel.ValueObjects.Ids;

public class PetId
{
    public PetId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static PetId Create(Guid value) => new(value);

    public static PetId NewPetId() => new(Guid.NewGuid());

    public static PetId Empty() => new(Guid.Empty);
}