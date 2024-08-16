namespace FindYourFriendAmongPets.Core.Models;

public record PetDetails
{
    public List<Requisite> Requisites { get; private set; }

    public List<PetPhoto> PetPhotos { get; private set; }
}