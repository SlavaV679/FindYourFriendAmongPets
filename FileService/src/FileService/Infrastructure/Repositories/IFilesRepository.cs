using FileService.Core;

namespace FileService.Infrastructure.Repositories;

public interface IFilesRepository
{
    Task<List<FileMetadata>> GetByIds(IEnumerable<Guid> fileDataIds, CancellationToken cancellationToken = default);
    
    Task<List<FileMetadata>> GetByDate(DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken = default);

    Task<FileMetadata?> GetById(Guid id, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<FileMetadata> fileData, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(IEnumerable<Guid> fileDataIds, CancellationToken cancellationToken = default);
}