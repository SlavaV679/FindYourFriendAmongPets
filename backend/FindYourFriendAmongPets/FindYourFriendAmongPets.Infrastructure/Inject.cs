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
        services
            .AddMinio(configuration)
            .AddHostedServices()
            .AddMessageQueues()
            .AddServices();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IFilesCleanerService, FilesCleanerService>();

        return services;
    }

    private static IServiceCollection AddMessageQueues(this IServiceCollection services)
    {
        services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>, InMemoryMessageQueue<IEnumerable<FileInfo>>>();

        return services;
    }

    private static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<FilesCleanerBackgroundService>();

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