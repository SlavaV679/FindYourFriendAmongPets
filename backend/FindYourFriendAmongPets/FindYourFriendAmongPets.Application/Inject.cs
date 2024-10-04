using FindYourFriendAmongPets.Application.Volunteers.Commands.AddPet;
using FindYourFriendAmongPets.Application.Volunteers.Commands.Create;
using FindYourFriendAmongPets.Application.Volunteers.Commands.Delete;
using FindYourFriendAmongPets.Application.Volunteers.Commands.UpdateMainInfo;
using FindYourFriendAmongPets.Application.Volunteers.Commands.UploadFilesToPet;
using FindYourFriendAmongPets.Application.Volunteers.Queries.GetPetsWithPaginationQuery;
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
        
        services.AddScoped<GetPetsWithPaginationHandler>();
        
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        
        return services;
    }
}