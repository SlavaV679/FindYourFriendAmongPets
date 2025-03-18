namespace FileService.Infrastructure.Options;

public class MongoDbOptions
{
    public static readonly string Mongo = nameof(Mongo);

    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string FilesCollectionName { get; set; } = null!;
}