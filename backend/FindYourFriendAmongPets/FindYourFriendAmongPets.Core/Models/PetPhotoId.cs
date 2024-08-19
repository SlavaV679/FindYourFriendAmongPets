namespace FindYourFriendAmongPets.Core.Models;

public class PetPhotoId
{
    protected PetPhotoId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static PetPhotoId Create(Guid value) => new(value);

    public static PetPhotoId NewPetPhotoId() => new(Guid.NewGuid());

    public static PetPhotoId Empty() => new(Guid.Empty);
}