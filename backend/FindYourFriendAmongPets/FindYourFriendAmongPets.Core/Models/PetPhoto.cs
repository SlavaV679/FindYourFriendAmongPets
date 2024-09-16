namespace FindYourFriendAmongPets.Core.Models;

public class PetPhoto
{
    // ef core
    private PetPhoto()
    {
    }

    public PetPhoto(FilePath pathToStorage, bool isMain = false)
    {
        PathToStorage = pathToStorage;
        IsMain = isMain;
    }

    public FilePath PathToStorage { get; }

    public bool IsMain { get; }
}