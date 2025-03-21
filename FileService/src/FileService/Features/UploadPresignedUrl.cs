﻿using FileService.Core;
using FileService.Endpoints;
using FileService.Infrastructure.Providers;

namespace FileService.Features;

public static class UploadPresignedUrl
{
    private record UploadPresignedUrlRequest(
        string BucketName,
        string FileName,
        string ContentType,
        string Prefix,
        string Extension);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/presigned", Handler);
        }
    }

    private static async Task<IResult> Handler(
        UploadPresignedUrlRequest request,
        IFileProvider fileProvider,
        CancellationToken cancellationToken)
    {
        var key = Guid.NewGuid();

        var fileMetadata = new FileMetadata
        {
            BucketName = request.BucketName,
            ContentType = request.ContentType,
            Name = request.FileName,
            Prefix = request.Prefix,
            Key = $"{key}.{request.Extension}"
        };

        var result = await fileProvider.GetPreSignedUrlForUpload(fileMetadata, cancellationToken);
        return Results.Ok(new
        {
            key,
            url = result.Value
        });
    }
}