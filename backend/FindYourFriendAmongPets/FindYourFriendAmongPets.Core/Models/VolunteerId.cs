namespace FindYourFriendAmongPets.Core.Models;

public record VolunteerId
{
    public VolunteerId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static VolunteerId Create(Guid value) => new(value);

    public static VolunteerId NewVolunteerId() => new(Guid.NewGuid());

    public static VolunteerId Empty() => new(Guid.Empty);
}