using Microsoft.EntityFrameworkCore;
using PetFriend.Accounts.Domain;
using PetFriend.Accounts.Infrastructure.DbContexts;

namespace PetFriend.Accounts.Infrastructure.IdentityManagers;

public class PermissionManager(AccountsWriteDbContext context)
{
    public async Task<Permission?> FindByCodeAsync(string code, CancellationToken cancellationToken = default) =>
        await context.Permissions.FirstOrDefaultAsync(p => p.Code == code, cancellationToken);

    public async Task AddRangeIfExistsAsync(IEnumerable<string> permissions, CancellationToken cancellationToken)
    {
        var uniquePermissions = permissions.ToList().Distinct();
        foreach (var permissionCode in uniquePermissions)
        {
            var isPermissionExists = await context.Permissions
                .AnyAsync(p => p.Code == permissionCode, cancellationToken);

            if (isPermissionExists is false)
                await context.Permissions.AddAsync(new Permission() { Code = permissionCode }, cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<string>?> GetPermissionsByUserId(
        Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .Include(u => u.Role)
            .ThenInclude(r => r.RolePermissions)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        return user?.Role.RolePermissions.Select(rp => rp.Permission.Code);
    }
}