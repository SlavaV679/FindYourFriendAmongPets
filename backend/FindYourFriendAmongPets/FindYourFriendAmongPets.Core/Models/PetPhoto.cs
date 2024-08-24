using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Core.Shared;

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

    public static Result<PetPhoto, Error> Create(PetPhotoId id, string pathToStorage, bool isMain = false)
    {
        if (string.IsNullOrWhiteSpace(pathToStorage))
            return Errors.General.ValueIsInvalid(nameof(PathToStorage), $"{nameof(PathToStorage)} can not be empty.");

        return new PetPhoto(id, pathToStorage, isMain);
    }
}