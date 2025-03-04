using FileService.Core;
using MongoDB.Driver;

namespace FileService.MongoDataAccess;

public class FileMongoDbContext(IMongoClient client)
{
    private readonly IMongoDatabase _database = client.GetDatabase("files_db");

    public IMongoCollection<FileData> Files => _database.GetCollection<FileData>("files");
}