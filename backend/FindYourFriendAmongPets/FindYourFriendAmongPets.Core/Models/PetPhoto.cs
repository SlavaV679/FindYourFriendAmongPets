using CSharpFunctionalExtensions;

namespace FindYourFriendAmongPets.Core.Models;

public class PetPhoto : Shared.Entity<PetPhotoId>
{
    // ef core
    private PetPhoto(PetPhotoId id) : base(id)
    {
    }

    private PetPhoto(PetPhotoId id, string pathToStorage, bool isMain)
        : base(id)
    {
        PathToStorage = pathToStorage;
        IsMain = isMain;
    }

    public string PathToStorage { get; }

    public bool IsMain { get; }

    public static Result<PetPhoto> Create(PetPhotoId id, string pathToStorage, bool isMain = false)
    {
        if (string.IsNullOrWhiteSpace(pathToStorage))
            return Result.Failure<PetPhoto>($"{nameof(PathToStorage)} can not be empty.");

        return Result.Success(new PetPhoto(id, pathToStorage, isMain));
    }
}