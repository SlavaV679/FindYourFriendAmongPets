using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFriend.Accounts.Infrastructure.DbContexts;

namespace PetFriend.Accounts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsInfrastructure(
        this IServiceCollection collection, IConfiguration configuration)
    {
        return collection;
    }
    
    private static IServiceCollection AddDbContexts(this IServiceCollection services)
    {
        services.AddScoped<AccountsWriteDbContext>();

        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        //services.AddScoped<IVolunteerRepository, VolunteerRepository>();
        
        return services;
    }
}