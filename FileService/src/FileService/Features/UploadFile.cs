using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using FileService.Core;
using FileService.Endpoints;
using FileService.Infrastructure.Providers;
using FileService.Infrastructure.Repositories;
using FileService.Jobs;
using Hangfire;

namespace FileService.Features;

public static class UploadFile
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/uploadfile", Handler).DisableAntiforgery();
        }
    }

    private static async Task<IResult> Handler(
        IFormFile file,
        IAmazonS3 s3Client,
        IFileProvider fileProvider,
        IFilesRepository filesRepository,
        CancellationToken cancellationToken)
    {
        try
        {
            var bucketName = "files";
            var key = Guid.NewGuid();
            var prefix = "testFolder";
            var fileId = Guid.NewGuid();

            var jobId = BackgroundJob
                .Schedule<ConsistencyConfirmJob>(
                    j => j.Execute(fileId, bucketName, key.ToString()),
                    TimeSpan.FromMinutes(15));

            var fileMetadata = new FileMetadata
            {
                BucketName = bucketName,
                ContentType = file.ContentType,
                Name = file.FileName,
                Prefix = prefix,
                Key = $"{prefix}/{key}",
                Size = file.Length
            };

            using var stream = file.OpenReadStream();

            var request = new PutObjectRequest
            {
                BucketName = fileMetadata.BucketName,
                Key = fileMetadata.Key,
                InputStream = stream,
                ContentType = fileMetadata.ContentType
            };

            try
            {
                var response = await s3Client.PutObjectAsync(request);
                if (response.HttpStatusCode != HttpStatusCode.OK)
                    return Results.StatusCode(500);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(500);
            }

            var downloadUrl = await fileProvider.GetPreSignedUrlForDownload(
                fileMetadata, cancellationToken);
            if (downloadUrl.IsFailure)
                return Results.BadRequest(downloadUrl.Error.Errors);

            fileMetadata.Id = fileId;
            fileMetadata.DownloadUrl = downloadUrl.Value;
            fileMetadata.Size = fileMetadata.Size;
            fileMetadata.CreatedDate = DateTime.UtcNow;

            await filesRepository.AddRangeAsync([fileMetadata], cancellationToken);

            BackgroundJob.Delete(jobId);

            return Results.Ok(new
            {
                key = key,
                // location = response.Location
            });
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3 error completing multipart upload: {ex.Message}");
        }
        // TODO отловить exception от MongoDB
    }
}