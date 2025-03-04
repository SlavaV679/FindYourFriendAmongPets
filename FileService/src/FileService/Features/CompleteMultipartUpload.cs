using Amazon.S3;
using Amazon.S3.Model;
using FileService.Core;
using FileService.Endpoints;
using FileService.Jobs;
using FileService.MongoDataAccess;
using Hangfire;

namespace FileService.Features;

public static class CompleteMultipartUpload
{
    private record PartETagInfo(int PartNumber, string ETag);

    private record CompleteMultipartRequest(string UploadId, List<PartETagInfo> Parts);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key}/complete-multipart", Handler);
        }
    }

    private static async Task<IResult> Handler(
        CompleteMultipartRequest request,
        string key,
        IAmazonS3 s3Client,
        IFileRepository fileRepository,
        CancellationToken cancellationToken)
    {
        try
        {
            var fileId = Guid.NewGuid();

            var jobId = BackgroundJob.Schedule<ConsistencyConfirmJob>(j => j.Execute(fileId, key), TimeSpan.FromSeconds(5));

            var completeRequest = new CompleteMultipartUploadRequest()
            {
                BucketName = "files",
                Key = key,
                UploadId = request.UploadId,
                PartETags = request.Parts.Select(x => new PartETag(x.PartNumber, x.ETag)).ToList()
            };

            var response = await s3Client.CompleteMultipartUploadAsync(
                completeRequest, cancellationToken);

            var metadataRequest = new GetObjectMetadataRequest()
            {
                BucketName = "files",
                Key = key
            };

            var metadata = await s3Client.GetObjectMetadataAsync(metadataRequest, cancellationToken);

            var fileData = new FileData
            {
                Id = fileId,
                StoragePath = key,
                UploadDate = DateTime.UtcNow,
                Size = metadata.Headers.ContentLength,
                ContentType = metadata.Headers.ContentType
            };

            await fileRepository.AddFileAsync(fileData, cancellationToken);

            BackgroundJob.Delete(jobId);

            return Results.Ok(new
            {
                Id = key,
                location = response.Location
            });
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3 error completing multipart upload: {ex.Message}");
        }
        // TODO отловить exception от MongoDB
    }
}