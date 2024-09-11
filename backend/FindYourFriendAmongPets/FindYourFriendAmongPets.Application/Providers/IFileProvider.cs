using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Application.FileProvider;
using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Shared;

namespace FindYourFriendAmongPets.Application.Providers;

public interface IFileProvider
{
    Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(IEnumerable<FileData> filesData, CancellationToken cancellationToken = default);
}