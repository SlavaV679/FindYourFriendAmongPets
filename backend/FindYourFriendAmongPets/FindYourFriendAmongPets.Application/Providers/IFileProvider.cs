using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Application.FileProvider;
using FindYourFriendAmongPets.Core.Shared;

namespace FindYourFriendAmongPets.Application.Providers;

public interface IFileProvider
{
    Task<Result<string, Error>> UploadFile(FileData fileData, CancellationToken cancellationToken = default);
}