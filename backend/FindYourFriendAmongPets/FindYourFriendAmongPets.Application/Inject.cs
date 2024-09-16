using FindYourFriendAmongPets.Application.Volunteers.AddPet;
using FindYourFriendAmongPets.Application.Volunteers.Create;
using FindYourFriendAmongPets.Application.Volunteers.UpdateMainInfo;
using FindYourFriendAmongPets.Application.Volunteers.Delete;
using FindYourFriendAmongPets.Application.Volunteers.UploadFilesToPet;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FindYourFriendAmongPets.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();

        services.AddScoped<UpdateMainInfoHandler>();
        
        services.AddScoped<DeleteVolunteerHandler>();
        
        services.AddScoped<AddPetHandler>();
        
        services.AddScoped<UploadFilesToPetHandler>();
        
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        
        return services;
    }
}