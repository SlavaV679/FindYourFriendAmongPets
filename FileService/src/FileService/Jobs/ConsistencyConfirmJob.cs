using Amazon.S3;
using FileService.MongoDataAccess;

namespace FileService.Jobs;

public class ConsistencyConfirmJob(
    IFileRepository fileRepository, 
    IAmazonS3 s3Client,
    ILogger<ConsistencyConfirmJob> logger)
{
    public async Task Execute(Guid fileId, string key)
    {
        logger.LogInformation("Starting consistency confirm job with fileId {FileId} and key {Key}.", fileId, key);
        // TODO сходить в MinIO, проверить что файл с fileId есть в папке с key
        // сходить MongoDB и проверить, что данные файла с fileId есть в MongoDB
        // и если где то данных нет (система не консистентна), тогда удалить существующие неконсистентные данные.
        await Task.Delay(3000);

        logger.LogInformation("Consistency confirm job finished.");
    }
}