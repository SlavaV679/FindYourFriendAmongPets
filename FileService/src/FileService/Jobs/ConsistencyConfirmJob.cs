using FileService.Infrastructure.Providers;
using FileService.Infrastructure.Repositories;
using Hangfire;

namespace FileService.Jobs;

public class ConsistencyConfirmJob(
    IFilesRepository filesRepository,
    IFileProvider fileProvider,
    ILogger<ConsistencyConfirmJob> logger)
{
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = [5, 10, 15])]
    public async Task Execute(Guid fileId, string bucketName, string key)
    {
        try
        {
            logger.LogInformation("Start ConsistencyConfirmJob with {fileId} and {key}", fileId, key);

            var metadataResult = await fileProvider.GetObjectMetadata(bucketName, key);
            if (metadataResult.IsFailure)
            {
                logger.LogWarning("Metadata not found for fileId: {fileId}.", fileId);
            }

            var mongoData = await filesRepository.GetById(fileId);

            if (mongoData is null)
            {
                logger.LogWarning("MongoDB record not found for fileId: {fileId}." +
                                  " Deleting file from cloud storage.", fileId);
                await fileProvider.DeleteFile(metadataResult.Value);
                return;
            }

            if (metadataResult.Value.Key != mongoData.Key)
            {
                logger.LogWarning("Metadata key does not match MongoDB data." +
                                  " Deleting file from cloud storage and MongoDB record.");

                await fileProvider.DeleteFile(metadataResult.Value);
                await filesRepository.DeleteRangeAsync([fileId]);
            }

            logger.LogInformation("End ConfirmConsistencyJob");
        }
        catch (Exception ex)
        {
            logger.LogError("Cannot check consistency, because " + ex.Message);
        }
    }
}