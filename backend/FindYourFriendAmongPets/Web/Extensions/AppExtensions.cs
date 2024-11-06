using Microsoft.EntityFrameworkCore;
using PetFriend.Volunteers.Infrastructure.DbContexts;

namespace Web.Extensions;

public static class AppExtensions
{
    public static async Task ApplyMigration(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var writeDbContext = scope.ServiceProvider.GetRequiredService<PetFamilyWriteDbContext>();

        await writeDbContext.Database.MigrateAsync();
    }
}