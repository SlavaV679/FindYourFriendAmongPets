﻿using FindYourFriendAmongPets.Application.Volunteers;
using FindYourFriendAmongPets.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FindYourFriendAmongPets.DataAccess;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<PetFamilyDbContext>();

        services.AddScoped<IVolunteerRepository, VolunteerRepository>();

        return services;
    }
}