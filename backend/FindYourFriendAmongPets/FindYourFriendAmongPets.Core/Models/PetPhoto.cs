using CSharpFunctionalExtensions;

namespace FindYourFriendAmongPets.Core.Models;

public record PetPhoto
{
    private PetPhoto(PetPhotoId id, string pathToStorage, bool isMain)
    {
        Id = id;
        PathToStorage = pathToStorage;
        IsMain = isMain;
    }

    public PetPhotoId Id { get; }

    public string PathToStorage { get; }

    public bool IsMain { get; }

    public static Result<PetPhoto> Create(PetPhotoId id, string pathToStorage, bool isMain)
    {
        if (string.IsNullOrWhiteSpace(pathToStorage))
            return Result.Failure<PetPhoto>($"{nameof(PathToStorage)} can not be empty.");

        return Result.Success(new PetPhoto(id, pathToStorage, isMain));
    }
}