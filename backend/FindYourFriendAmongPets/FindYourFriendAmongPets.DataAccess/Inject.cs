using FindYourFriendAmongPets.Application.Database;
using FindYourFriendAmongPets.Application.Volunteers;
using FindYourFriendAmongPets.DataAccess.DBContexts;
using FindYourFriendAmongPets.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FindYourFriendAmongPets.DataAccess;

public static class Inject
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        services
            .AddDbContexts()
            .AddRepositories()
            .AddDatabase();

        return services;
    }
    
    private static IServiceCollection AddDbContexts(this IServiceCollection services)
    {
        services.AddScoped<PetFamilyWriteDbContext>();
        
        services.AddScoped<IReadDbContext, ReadDbContext>();

        return services;
    }
    
    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        
        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVolunteerRepository, VolunteerRepository>();
        
        return services;
    }
}