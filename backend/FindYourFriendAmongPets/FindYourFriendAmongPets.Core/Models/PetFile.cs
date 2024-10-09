namespace FindYourFriendAmongPets.Core.Models;

public class PetFile
{
    // ef core
    private PetFile()
    {
    }

    public PetFile(FilePath pathToStorage, bool isMain = false)
    {
        PathToStorage = pathToStorage;
        IsMain = isMain;
    }

    public FilePath PathToStorage { get; }

    public bool IsMain { get; }
}