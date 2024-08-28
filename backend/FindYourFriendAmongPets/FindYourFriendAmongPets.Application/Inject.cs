using FindYourFriendAmongPets.Application.Volunteers.Create;
using Microsoft.Extensions.DependencyInjection;

namespace FindYourFriendAmongPets.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();

        return services;
    }
}