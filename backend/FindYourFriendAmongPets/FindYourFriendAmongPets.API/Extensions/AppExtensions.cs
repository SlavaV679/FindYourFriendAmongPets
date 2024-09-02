using FindYourFriendAmongPets.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace FindYourFriendAmongPets.API.Extensions;

public static class AppExtensions
{
    public static async Task ApplyMigration(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PetFamilyDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}