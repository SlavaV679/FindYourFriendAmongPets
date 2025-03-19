using FileService.Core;
using MongoDB.Driver;

namespace FileService.Infrastructure.Repositories;

public class FilesRepository : IFilesRepository
{
    private readonly FileMongoDbContext _mongoDbContext;
private readonly ILogger<FilesRepository> _logger;

    public FilesRepository(FileMongoDbContext mongoDbContext, ILogger<FilesRepository> logger)
    {
        _mongoDbContext = mongoDbContext;
        _logger = logger;
    }

    public async Task<List<FileMetadata>> Get(IEnumerable<Guid> fileDataIds, CancellationToken cancellationToken = default)
    {
        return await _mongoDbContext.Files.Find(file => fileDataIds.Contains(file.Id)).ToListAsync(cancellationToken);
    }

    public async Task<FileMetadata?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await _mongoDbContext.Files.Find(file => file.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<FileMetadata> fileData, CancellationToken cancellationToken = default)
    {
        _logger.LogWarning("AddRangeAsync started.");
        await _mongoDbContext.Files.InsertManyAsync(fileData, cancellationToken: cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<Guid> fileDataIds, CancellationToken cancellationToken = default)
    {
        await _mongoDbContext.Files.DeleteManyAsync(file => fileDataIds.Contains(file.Id), cancellationToken);
    }
}