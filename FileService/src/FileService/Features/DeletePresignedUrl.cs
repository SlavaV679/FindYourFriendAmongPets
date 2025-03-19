using FileService.Core;
using FileService.Endpoints;
using FileService.Infrastructure.Providers;
using FileService.Infrastructure.Repositories;

namespace FileService.Features;

public static class DeletePresignedUrl
{
    private record DeletePresignedUrlRequest(
        string BucketName,
        string FileName,
        string Prefix,
        string ContentType);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key:guid}/presigned-for-deletion", Handler);
        }
    }

    private static async Task<IResult> Handler(
        DeletePresignedUrlRequest request,
        Guid key,
        IFilesRepository filesRepository,
        IFileProvider provider,
        CancellationToken cancellationToken = default)
    {
        var fileMetadata = new FileMetadata
        {
            BucketName = request.BucketName,
            Name = request.FileName,
            Key = $"{request.Prefix}/{key}",
            ContentType = request.ContentType
        };

        var result = await provider.GetPreSignedUrlForDelete(fileMetadata, cancellationToken);

        return Results.Ok(new
        {
            key,
            url = result.Value
        });
    }
}