namespace FindYourFriendAmongPets.Core.Models;

public record PetPhotosList
{
    private PetPhotosList()
    {
    }

    public PetPhotosList(IEnumerable<PetPhoto> petPhotos)
    {
        PetPhotos = petPhotos.ToList();
    }

    public IReadOnlyList<PetPhoto> PetPhotos { get; }
}