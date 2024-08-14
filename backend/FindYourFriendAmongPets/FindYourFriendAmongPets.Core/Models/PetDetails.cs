namespace FindYourFriendAmongPets.Core.Models;

public record PetDetails
{
    public List<PetPhoto> PetPhotos { get; private set; }
}