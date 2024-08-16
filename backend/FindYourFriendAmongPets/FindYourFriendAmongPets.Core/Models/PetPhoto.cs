using CSharpFunctionalExtensions;

namespace FindYourFriendAmongPets.Core.Models;

public record PetPhoto
{
    // ef core
    private PetPhoto()
    {
        
    }
    private PetPhoto(string pathToStorage, bool isMain)
    {
        PathToStorage = pathToStorage;
        IsMain = isMain;
    }

    public string PathToStorage { get; }

    public bool IsMain { get; }

    public static Result<PetPhoto> Create(string pathToStorage, bool isMain)
    {
        if (string.IsNullOrWhiteSpace(pathToStorage))
            return Result.Failure<PetPhoto>($"{nameof(PathToStorage)} can not be empty.");

        return Result.Success(new PetPhoto(pathToStorage, isMain));
    }
}