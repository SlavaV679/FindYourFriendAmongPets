using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFriend.Accounts.Domain;
using PetFriend.Accounts.Domain.TypeAccounts;
using PetFriend.Accounts.Infrastructure.IdentityManagers;
using PetFriend.Framework;
using PetFriend.SharedKernel.ValueObjects;

namespace PetFriend.Accounts.Infrastructure.Seeding;

public class AccountsSeederService(
    PermissionManager permissionManager,
    RolePermissionManager rolePermissionManager,
    RoleManager<Role> roleManager,
    UserManager<User> userManager,
    AdminAccountManager adminAccountManager,
    IOptions<AdminOptions> adminOptions,
    ILogger<AccountsSeeder> logger)
{
    private const string FILE_PATH = "etc/accounts.json";

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var json = await File.ReadAllTextAsync(FILE_PATH, cancellationToken);

        var seedData = JsonSerializer.Deserialize<RolePermissionOptions>(json) ??
                       throw new ArgumentNullException(nameof(json));

        await SeedRoles(seedData);
        await SeedPermissionsAsync(seedData, cancellationToken);
        await SeedRolePermissions(seedData, cancellationToken);
        await SeedAdminAsync(cancellationToken);
    }

    private async Task SeedAdminAsync(CancellationToken cancellationToken = default)
    {
        var isAdminExists = await userManager.FindByEmailAsync(adminOptions.Value.Email);
        if (isAdminExists is not null)
            return;

        logger.LogInformation("Seeding admin...");
        var role = await roleManager.FindByNameAsync(Roles.Admin);
        if (role is null)
            throw new ArgumentException("admin role not exists");

        var fullName = FullName.Create(adminOptions.Value.Name,
            adminOptions.Value.Surname,
            adminOptions.Value.Patronymic);

        if (fullName.IsFailure)
            throw new ArgumentException(fullName.Error.Message);

        var user = User.CreateAdmin(fullName.Value, adminOptions.Value.UserName, adminOptions.Value.Email, role);
        await userManager.CreateAsync(user, adminOptions.Value.Password);

        var adminAccount = new AdminAccount(user);

        await adminAccountManager.CreateAdminAccountAsync(adminAccount, cancellationToken);

        logger.LogInformation("Seeding admin done.");
    }

    private async Task SeedRolePermissions(
        RolePermissionOptions seedData,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("RolePermission adding to database...");

        foreach (var roleName in seedData.Roles.Keys)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role is null)
                throw new ArgumentNullException(nameof(roleName));

            await rolePermissionManager.AddRangeIfExistAsync(role!.Id, seedData.Roles[roleName], cancellationToken);
        }

        logger.LogInformation("RolePermission adding to database done.");
    }

    private async Task SeedRoles(RolePermissionOptions seedData)
    {
        logger.LogInformation("Seeding roles...");

        foreach (var role in seedData.Roles.Keys)
        {
            var isRoleExists = await roleManager.FindByNameAsync(role);
            if (isRoleExists is null)
                await roleManager.CreateAsync(new Role { Name = role });
        }

        logger.LogInformation("Seeding roles done.");
    }

    private async Task SeedPermissionsAsync(
        RolePermissionOptions seedData,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Seeding permissions...");

        var permissions = seedData.Permissions.SelectMany(pg => pg.Value);
        await permissionManager.AddRangeIfExistsAsync(permissions, cancellationToken);

        logger.LogInformation("Seeding permissions done.");
    }
}