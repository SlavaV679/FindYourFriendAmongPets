using Amazon.S3;
using Amazon.S3.Model;
using FileService.Endpoints;

namespace FileService.Features;

public static class UploadPresignedUrl
{
    private record UploadPresignedUrlRequest(string FileName, string ContentType, long Size);
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/presigned", Handler);
        }
    }

    private static async Task<IResult> Handler(
        UploadPresignedUrlRequest request,
        IAmazonS3 s3Client,
        CancellationToken cancellationToken)
    {
        try
        {
            var key = Guid.NewGuid();
            var presignedRequest = new GetPreSignedUrlRequest
            {
                BucketName = "files",
                Key = $"files/{key}",
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddHours(24),
                ContentType = request.ContentType,
                Protocol = Protocol.HTTP,
                Metadata =
                {
                    ["file-name"] = request.FileName
                }
            };
 
            var url = await s3Client.GetPreSignedURLAsync(presignedRequest);
            
            return Results.Ok(new
            {
                key,
                url
            });
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3 error generating presigned URL: {ex.Message}");
        }
    }
}