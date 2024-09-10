using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Application.FileProvider;
using FindYourFriendAmongPets.Core.Shared;

namespace FindYourFriendAmongPets.Application.Providers;

public interface IFileProvider
{
    Task<UnitResult<Error>> UploadFiles(FileData fileData, CancellationToken cancellationToken = default);
}