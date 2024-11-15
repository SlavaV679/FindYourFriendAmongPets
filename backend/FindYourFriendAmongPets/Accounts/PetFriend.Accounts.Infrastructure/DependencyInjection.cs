using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFriend.Accounts.Domain;
using PetFriend.Accounts.Infrastructure.DbContexts;

namespace PetFriend.Accounts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContexts()
            .AddIdentityServices();
        return services;
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
    
    private static IServiceCollection AddIdentityServices(this IServiceCollection collection)
    {
        collection.AddIdentity<User, Role>(options =>
            {
                // options.User.RequireUniqueEmail = true;
                // options.Password.RequireDigit = false;
                // options.Password.RequireLowercase = false;
                // options.Password.RequireNonAlphanumeric = false;
                // options.Password.RequireUppercase = false;
                // options.Password.RequiredLength = 6;
                // options.Password.RequiredUniqueChars = 0;
            })
            .AddEntityFrameworkStores<AccountsWriteDbContext>();

        return collection;
    }
}