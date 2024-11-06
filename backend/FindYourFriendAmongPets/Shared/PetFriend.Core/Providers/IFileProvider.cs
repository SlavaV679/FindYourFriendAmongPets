using CSharpFunctionalExtensions;
using PetFriend.SharedKernel;
using PetFriend.SharedKernel.ValueObjects;

namespace PetFriend.Core.Providers;

public interface IFileProvider
{
    Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(IEnumerable<FileData> filesData, CancellationToken cancellationToken = default);

    Task<Result<string, Error>> GetFileByName(string fileName, string bucketName, CancellationToken token = default);

    Task<Result<string, Error>> Delete(string fileName, string bucketName, CancellationToken token = default);

    Task<UnitResult<Error>> RemoveFile(FileInfo fileInfo, CancellationToken cancellationToken = default);
}