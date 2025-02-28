using FileService.Core;
using FileService.Endpoints;
using FileService.Jobs;
using FileService.MongoDataAccess;
using Hangfire;

namespace FileService.Features;

public static class TestHangFire
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("hangfire-test", Handler);
        }
    }

    private static async Task<IResult> Handler(
        IFileRepository fileRepository,
        CancellationToken cancellationToken)
    {
        var fileData = new FileData
        {
            Id = Guid.NewGuid(),
            StoragePath = "key",
            UploadDate = DateTime.UtcNow,
            Size = 100,
            ContentType = "metadata.Headers.ContentType"
        };

        // Test of inserting info about file into mongo db 
        await fileRepository.AddFileAsync(fileData, cancellationToken);

        var jobId = BackgroundJob.Schedule<ConsistencyConfirmJob>(j =>
            j.Execute(Guid.NewGuid(), "key"), TimeSpan.FromSeconds(5));

        return Results.Ok(jobId);
    }
}