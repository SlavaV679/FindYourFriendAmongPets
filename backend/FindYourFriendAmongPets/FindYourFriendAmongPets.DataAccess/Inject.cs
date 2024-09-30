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
        services.AddScoped<PetFamilyWriteDbContext>();

        services.AddScoped<IVolunteerRepository, VolunteerRepository>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}