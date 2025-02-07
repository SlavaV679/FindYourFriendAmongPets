using Amazon.S3;
using Amazon.S3.Model;
using FileService.Endpoints;

namespace FileService.Features;

public static class CompleteMultipartUpload
{
    private record PartETagInfo(int PartNumber, string ETag);
    
    private record CompleteMultipartRequest(string UploadId, List<PartETagInfo> Parts);
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key:guid}/complete-multipart", Handler);
        }
    }  

    private static async Task<IResult> Handler(
        CompleteMultipartRequest request,
        Guid key,
        IAmazonS3 s3Client,
        CancellationToken cancellationToken)
    {
        try
        {
            var completeRequest = new CompleteMultipartUploadRequest()
            {
                BucketName = "files",
                Key = $"files/{key}",
                UploadId = request.UploadId,
                PartETags = request.Parts.Select(x => new PartETag(x.PartNumber, x.ETag)).ToList()
            };
 
            var response = await s3Client.CompleteMultipartUploadAsync(
                completeRequest, cancellationToken);
            
            // TODO: Insert into mongo db info about file
            
            return Results.Ok(new
            {
                key,
                location = response.Location
            });
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3 error completing multipart upload: {ex.Message}");
        }
    }
}