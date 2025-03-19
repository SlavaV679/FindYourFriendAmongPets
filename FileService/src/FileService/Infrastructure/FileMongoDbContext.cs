using FileService.Core;
using FileService.Infrastructure.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FileService.Infrastructure;

public class FileMongoDbContext
{
    private readonly IMongoDatabase _database;
    private readonly MongoDbOptions _options;

    public FileMongoDbContext(IOptions<MongoDbOptions> options)
    {
        _options = options.Value;
        var client = new MongoClient(_options.ConnectionString);
        _database = client.GetDatabase(_options.DatabaseName);
    }

    public IMongoCollection<FileMetadata> Files => _database.GetCollection<FileMetadata>(_options.FilesCollectionName);
}