using FileService.Core;
using FileService.Endpoints;
using FileService.Infrastructure.Providers;

namespace FileService.Features;

public static class DownloadPresignedUrl
{
    private record DownloadPresignedUrlRequest(
        string BucketName,
        string Prefix);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key:guid}/presigned-for-downloading", Handler);
        }
    }

    private static async Task<IResult> Handler(
        DownloadPresignedUrlRequest request,
        Guid key,
        IFileProvider provider,
        CancellationToken cancellationToken = default)
    {
        var fileMetadata = new FileMetadata
        {
            BucketName = request.BucketName,
            Prefix = request.Prefix,
            Key = $"{request.Prefix}/{key}",
        };

        var result = await provider.GetPreSignedUrlForDownload(fileMetadata, cancellationToken);
        
        return Results.Ok(new
        {
            key,
            url = result.Value
        });
    }
}