using Amazon.S3;
using Amazon.S3.Model;
using CSharpFunctionalExtensions;
using FileService.Core;
using FileService.Core.Models;

namespace FileService.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private const int MAX_DEGREE_OF_PARALLELISM = 10;
    private const int EXPIRY = 7; // days
    private readonly IAmazonS3 _client;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(
        IAmazonS3 client,
        ILogger<MinioProvider> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<InitiateMultipartUploadResponse> StartMultipartUpload(FileMetadata fileMetadata, CancellationToken cancellationToken = default)
    {
        var presignedRequest = new InitiateMultipartUploadRequest
        {
            BucketName = fileMetadata.BucketName,
            Key = fileMetadata.Key,
            ContentType = fileMetadata.ContentType,
            Metadata =
            {
                ["file-name"] = fileMetadata.Name
            }
        };

        var response = await _client.InitiateMultipartUploadAsync(presignedRequest, cancellationToken);

        return response;
    }

    public async Task<Result<string, ErrorList>> GetPreSignedUrlPart(FileMetadata fileMetadata, CancellationToken cancellationToken)
    {
        try
        {
            var presignedRequest = new GetPreSignedUrlRequest
            {
                BucketName = fileMetadata.BucketName,
                Key = fileMetadata.Key,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddDays(EXPIRY),
                Protocol = Protocol.HTTP,
                UploadId = fileMetadata.UploadId,
                PartNumber = fileMetadata.PartNumber,
            };

            var result = await _client.GetPreSignedURLAsync(presignedRequest);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload file in minio with path {path} in bucket {bucket}",
                fileMetadata.FullPath,
                fileMetadata.BucketName);

            return Errors.Files.FailUpload().ToErrorList();
        }
    }

    public async Task<CompleteMultipartUploadResponse> CompleteMultipartUpload(FileMetadata fileMetadata, CancellationToken cancellationToken = default)
    {
        var presignedRequest = new CompleteMultipartUploadRequest()
        {
            BucketName = fileMetadata.BucketName,
            Key = fileMetadata.Key,
            UploadId = fileMetadata.UploadId,
            PartETags = fileMetadata.ETags!.Select(e => new PartETag(e.PartNumber, e.ETag)).ToList()
        };

        var response = await _client.CompleteMultipartUploadAsync(presignedRequest, cancellationToken);

        return response;
    }

    public async Task<Result<string, ErrorList>> GetPreSignedUrlForUpload(
        FileMetadata fileMetadata,
        CancellationToken cancellationToken)
    {
        try
        {
            var presignedRequest = new GetPreSignedUrlRequest
            {
                BucketName = fileMetadata.BucketName,
                Key = fileMetadata.Key,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddDays(EXPIRY),
                ContentType = fileMetadata.ContentType,
                Protocol = Protocol.HTTP,
                Metadata =
                {
                    ["file-name"] = fileMetadata.Name
                }
            };

            var result = await _client.GetPreSignedURLAsync(presignedRequest);
            
            _logger.LogInformation("Result GetPreSignedUrlForUpload: {result}", result);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload file in minio with path {path} in bucket {bucket}",
                fileMetadata.FullPath,
                fileMetadata.BucketName);

            return Errors.Files.FailUpload().ToErrorList();
        }
    }

    public async Task<Result<string, ErrorList>> GetPreSignedUrlForDownload(FileMetadata fileMetadata, CancellationToken cancellationToken)
    {
        try
        {
            var preSignedRequest = new GetPreSignedUrlRequest
            {
                BucketName = fileMetadata.BucketName,
                Key = fileMetadata.Key,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddDays(EXPIRY),
                Protocol = Protocol.HTTP,
            };

            var url = await _client.GetPreSignedURLAsync(preSignedRequest);

            if (url is null)
                return Errors.Files.NotFound().ToErrorList();

            return url;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Fail to get file in minio");
            return Errors.Files.FailUpload().ToErrorList();
        }
    }

    public async Task<Result<FileMetadata, ErrorList>> GetObjectMetadata(string bucketName, string key, CancellationToken cancellationToken = default)
    {
        var metaDataRequest = new GetObjectMetadataRequest
        {
            BucketName = bucketName,
            Key = key
        };
        try
        {
            var metaDataResponse = await _client.GetObjectMetadataAsync(metaDataRequest, cancellationToken);

            var metaData = new FileMetadata
            {
                Id = Guid.NewGuid(),
                Key = key,
                Size = metaDataResponse.Headers.ContentLength,
                ContentType = metaDataResponse.Headers.ContentType,
                BucketName = bucketName
            };

            return metaData;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return Errors.Files.NotFound().ToErrorList();
        }
    }

    public async Task<Result<IReadOnlyCollection<string>, ErrorList>> DownloadFiles(IEnumerable<FileMetadata> filesData,
        CancellationToken cancellationToken = default)
    {
        var fileList = filesData.ToList();
        var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);

        await EnsureBucketExistsAsync(fileList.Select(f => f.BucketName), cancellationToken);

        var tasks = fileList.Select(async fileMetadata =>
            await GetPreSignedUrlForDownload(fileMetadata, semaphoreSlim, cancellationToken));
        var results = await Task.WhenAll(tasks);

        var errors = results
            .Where(r => r.IsFailure)
            .Select(r => r.Error).ToList();

        if (errors.Count > 0)
            return new ErrorList(errors);

        _logger.LogInformation("Files downloaded from Minio");
        return results.Select(r => r.Value).ToList();
    }

    public async Task<UnitResult<ErrorList>> DeleteFile(FileMetadata fileMetadata,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new DeleteObjectRequest
            {
                BucketName = fileMetadata.BucketName,
                Key = fileMetadata.Key
            };

            await _client.DeleteObjectAsync(request, cancellationToken);

            return Result.Success<ErrorList>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Fail to delete file in minio");
            return Errors.Files.FailRemove().ToErrorList();
        }
    }

    private async Task EnsureBucketExistsAsync(IEnumerable<string> bucketNames, CancellationToken cancellationToken)
    {
        HashSet<string> buckets = [..bucketNames];

        var response = await _client.ListBucketsAsync(cancellationToken);

        foreach (var bucketName in buckets)
        {
            var isExist = response.Buckets
                .Exists(b => b.BucketName.Equals(bucketName, StringComparison.OrdinalIgnoreCase));

            if (!isExist)
            {
                var request = new PutBucketRequest
                {
                    BucketName = bucketName
                };

                await _client.PutBucketAsync(request, cancellationToken);
            }
        }
    }

    private async Task<Result<string, Error>> GetPreSignedUrlForDownload(
        FileMetadata fileMetadata,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        try
        {
            var presignedRequest = new GetPreSignedUrlRequest()
            {
                BucketName = fileMetadata.BucketName,
                Key = fileMetadata.Key,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddDays(EXPIRY),
                Protocol = Protocol.HTTP
            };

            var url = await _client.GetPreSignedURLAsync(presignedRequest);
            return url;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to download files from Minio");
            return Errors.Files.FailUpload();
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    public async Task<Result<string, ErrorList>> GetPreSignedUrlForDelete(
        FileMetadata fileMetadata,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureBucketExistsAsync([fileMetadata.BucketName], cancellationToken);

            var deleteRequest = new GetPreSignedUrlRequest
            {
                BucketName = fileMetadata.BucketName,
                Key = fileMetadata.Key,
                Verb = HttpVerb.DELETE,
                Expires = DateTime.UtcNow.AddDays(EXPIRY),
                Protocol = Protocol.HTTP
            };

            var url = await _client.GetPreSignedURLAsync(deleteRequest);

            if (url is null)
                return Errors.Files.NotFound().ToErrorList();

            return url;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Fail to remove file in minio with path {path} in bucket {bucket}",
                fileMetadata.FullPath, fileMetadata.BucketName);
            return Errors.Files.FailRemove().ToErrorList();
        }
    }
}