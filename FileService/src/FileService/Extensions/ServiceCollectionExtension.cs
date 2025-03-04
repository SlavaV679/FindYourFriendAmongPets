using Hangfire;
using Hangfire.PostgreSql;

namespace FileService.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddHangfirePostgres(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config =>
            config.UsePostgreSqlStorage(c =>
                c.UseNpgsqlConnection(configuration.GetConnectionString("PetFamilyDb"))));

        return services;
    }
}