namespace FindYourFriendAmongPets.Core.Models;

public record PetPhotoId
{
    public PetPhotoId(Guid value)
    {
        Value = value;
    }
    public Guid Value { get; }
    
    public static PetPhotoId NewPetPhotoId() => new(Guid.NewGuid());

    public static PetPhotoId Empty() => new(Guid.Empty);
}