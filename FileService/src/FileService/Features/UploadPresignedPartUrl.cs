using Amazon.S3;
using Amazon.S3.Model;
using FileService.Endpoints;

namespace FileService.Features;

public static class UploadPresignedPartUrl
{
    private record UploadPresignedPartUrlRequest(string UploadId, int PartNumber);
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key:guid}/presigned-part", Handler);
        }
    }

    private static async Task<IResult> Handler(
        UploadPresignedPartUrlRequest request,
        Guid key,
        IAmazonS3 s3Client,
        CancellationToken cancellationToken)
    {
        try
        {
            var presignedRequest = new GetPreSignedUrlRequest
            {
                BucketName = "files",
                Key = $"files/{key}",
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddHours(24),
                Protocol = Protocol.HTTP,
                UploadId = request.UploadId,
                PartNumber = request.PartNumber
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