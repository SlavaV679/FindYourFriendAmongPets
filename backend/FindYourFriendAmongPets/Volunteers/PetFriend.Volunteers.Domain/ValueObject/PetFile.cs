using PetFriend.SharedKernel.ValueObjects;

namespace PetFriend.Volunteers.Domain.ValueObject;

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