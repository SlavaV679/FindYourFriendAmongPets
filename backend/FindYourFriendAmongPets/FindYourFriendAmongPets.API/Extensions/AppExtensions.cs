using FindYourFriendAmongPets.DataAccess;
using FindYourFriendAmongPets.DataAccess.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace FindYourFriendAmongPets.API.Extensions;

public static class AppExtensions
{
    public static async Task ApplyMigration(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var writeDbContext = scope.ServiceProvider.GetRequiredService<PetFamilyWriteDbContext>();

        await writeDbContext.Database.MigrateAsync();
    }
}