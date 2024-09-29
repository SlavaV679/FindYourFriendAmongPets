using FindYourFriendAmongPets.Application.Files;
using FindYourFriendAmongPets.Application.Messaging;
using FindYourFriendAmongPets.Infrastructure.BackgroundServices;
using FindYourFriendAmongPets.Infrastructure.Files;
using FindYourFriendAmongPets.Infrastructure.MessageQueues;
using FindYourFriendAmongPets.Infrastructure.Options;
using FindYourFriendAmongPets.Infrastructure.Provider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using FileInfo = FindYourFriendAmongPets.Application.Files.FileInfo;

namespace FindYourFriendAmongPets.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMinio(configuration);

        services.AddScoped<IFilesCleanerService, FilesCleanerService>();

        services.AddHostedService<FilesCleanerBackgroundService>();
        
        services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>, InMemoryMessageQueue<IEnumerable<FileInfo>>>();
        
        return services;
    }

    private static IServiceCollection AddMinio(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.MINIO));

        services.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                               ?? throw new ApplicationException("Missing minio configuration");
            options.WithEndpoint(minioOptions.Endpoint);
            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.WithSsl);
        });
        
        services.AddScoped<IFileProvider, MinioProvider>();
        
        return services;
    }
}