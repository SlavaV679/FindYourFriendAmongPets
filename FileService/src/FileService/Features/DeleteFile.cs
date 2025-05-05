using Amazon.S3;
using Amazon.S3.Model;
using FileService.Core;
using FileService.Endpoints;
using FileService.Infrastructure.Providers;
using FileService.Infrastructure.Repositories;

namespace FileService.Features;

public static class DeleteFile
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("files/{key:guid}/delete", Handler);
        }
    }

    private static async Task<IResult> Handler(
        Guid key,
        IFilesRepository filesRepository,
        IFileProvider provider,
        CancellationToken cancellationToken = default)
    {
        var fileMetadata = await filesRepository.GetById(key, cancellationToken);

        if (fileMetadata == null)
            return Results.BadRequest("File not founded");

        var result = await provider.DeleteFile(fileMetadata, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error.Errors);

        await filesRepository.DeleteRangeAsync([key], cancellationToken);

        return Results.Ok(new
        {
            key,
            resultMessage = "Entity successfully deleted"
        });
    }
}