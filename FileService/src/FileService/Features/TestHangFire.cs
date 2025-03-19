using FileService.Core;
using FileService.Endpoints;
using FileService.Infrastructure.Repositories;
using FileService.Jobs;
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
        IFilesRepository filesRepository,
        CancellationToken cancellationToken)
    {
        //throw new Exception("Checking Middleware of exception handle.");

        var fileMetadata = new FileMetadata
        {
            BucketName = "files",
            ContentType = "mpg4",
            Name = "FileName",
            Prefix = "Prefix",
            Key = "key_Extension"
        };

        // Test of inserting info about file into mongo db 
        await filesRepository.AddRangeAsync([fileMetadata], cancellationToken);

        var jobId = BackgroundJob.Schedule<ConsistencyConfirmJob>(j =>
            j.Execute(Guid.NewGuid(), fileMetadata.BucketName, "key"), TimeSpan.FromSeconds(5));

        return Results.Ok(jobId);
    }
}