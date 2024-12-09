using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFriend.Accounts.Application;
using PetFriend.Accounts.Contracts;
using PetFriend.Accounts.Infrastructure;

namespace PetFriend.Accounts.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsModule(this IServiceCollection collection, IConfiguration configuration)
    {
        return collection
            .AddScoped<IAccountContract, AccountContract>()
            .AddAccountsApplication()
            .AddAccountsInfrastructure(configuration);
    }
    
    
}