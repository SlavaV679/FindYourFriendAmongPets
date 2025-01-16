using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PetFriend.Accounts.Application;
using PetFriend.Accounts.Domain;
using PetFriend.Accounts.Infrastructure.DbContexts;
using PetFriend.Accounts.Infrastructure.IdentityManagers;
using PetFriend.Accounts.Infrastructure.Ooptions;
using PetFriend.Accounts.Infrastructure.Providers;
using PetFriend.Accounts.Infrastructure.Seeding;
using PetFriend.Framework.Authorization;
using PetFriend.Framework.Options;

namespace PetFriend.Accounts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContexts()
            .AddIdentityServices()
            .AddJwtAuthentication()
            .AddAuthServices()
            ;

        services.ConfigureOptions(configuration);

        services.AddSingleton<AccountsSeeder>()
            .AddScoped<AccountsSeederService>();

        services.AddScoped<IAccountsUnitOfWork, AccountsUnitOfWork>();

        return services;
    }

    private static IServiceCollection ConfigureOptions(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AdminOptions>(configuration.GetSection(AdminOptions.ADMIN));
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JWT));

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
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                // options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            })
            .AddEntityFrameworkStores<AccountsWriteDbContext>();

        collection.AddScoped<PermissionManager>()
            .AddScoped<RolePermissionManager>()
            .AddScoped<AdminAccountManager>()
            .AddScoped<IParticipantAccountManager, ParticipantAccountManager>()
            .AddScoped<IVolunteerAccountManager, VolunteerAccountManager>()
            .AddTransient<ITokenProvider, JwtTokenProvider>()
            .AddScoped<IRefreshSessionManager, RefreshSessionManager>()
            ;

        return collection;
    }

    private static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>()
            .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

        return services;
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtOptions = services
                    .BuildServiceProvider()
                    .GetRequiredService<IOptions<JwtOptions>>().Value;

                options.TokenValidationParameters = TokenValidationParametersFactory.CreateWithLifetime(jwtOptions);
            });

        services.AddAuthorization();

        return services;
    }
}