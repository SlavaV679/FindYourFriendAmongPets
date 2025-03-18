using FileService.Core;
using FileService.Endpoints;
using FileService.Infrastructure.Providers;

namespace FileService.Features;

public static class UploadPresignedPartUrl
{
    private record UploadPresignedPartUrlRequest(
        string UploadId,
        int PartNumber,
        string BucketName,
        string ContentType,
        string Prefix,
        string FileName);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key}/presigned-part", Handler);
        }
    }

    private static async Task<IResult> Handler(
        UploadPresignedPartUrlRequest request,
        string key,
        IFileProvider fileProvider,
        CancellationToken cancellationToken)
    {
        var fileMetadata = new FileMetadata
        {
            BucketName = request.BucketName,
            ContentType = request.ContentType,
            Name = request.FileName,
            Prefix = request.Prefix,
            Key = $"{request.Prefix}/{key}",
            UploadId = request.UploadId,
            PartNumber = request.PartNumber
        };


        var response = await fileProvider
            .GetPreSignedUrlPart(fileMetadata, cancellationToken);

        if (response.IsFailure)
            return Results.BadRequest(response.Error.Errors);

        return Results.Ok(new
        {
            url = response.Value
        });
    }
}