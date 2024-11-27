using Microsoft.Extensions.DependencyInjection;

namespace PetFriend.Accounts.Infrastructure.Seeding;

public class AccountsSeeder(IServiceScopeFactory serviceScopeFactory)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<AccountsSeederService>();
        await seeder.SeedAsync(cancellationToken);
    }
}