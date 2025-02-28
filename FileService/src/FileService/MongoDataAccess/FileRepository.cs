using CSharpFunctionalExtensions;
using FileService.Core;
using FileService.Core.Models;
using MongoDB.Driver;

namespace FileService.MongoDataAccess;

public class FileRepository : IFileRepository
{
    private readonly FileMongoDbContext _mongoDbContext;
    private readonly ILogger<FileRepository> _logger;

    public FileRepository(
        FileMongoDbContext mongoDbContext,
        ILogger<FileRepository> logger)
    {
        _mongoDbContext = mongoDbContext;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> AddFileAsync(FileData fileData, CancellationToken cancellationToken)
    {
        await _mongoDbContext.Files.InsertOneAsync(fileData, cancellationToken: cancellationToken);

        return fileData.Id;
    }

    public async Task<IReadOnlyCollection<FileData>> GetFilesAsync(IEnumerable<Guid> fileIds,
        CancellationToken cancellationToken)
    {
        return await _mongoDbContext.Files
            .Find(f => fileIds.Contains(f.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<UnitResult<Error>> DeleteMany(IEnumerable<Guid> fileIds, CancellationToken cancellationToken)
    {
        var deleteResult = await _mongoDbContext.Files
            .DeleteManyAsync(f =>
                fileIds.Contains(f.Id), cancellationToken: cancellationToken);
        
        if (deleteResult.DeletedCount == 0)
        {
            return Errors.Files.FailRemove();
        }

        return Result.Success<Error>();
    }
}