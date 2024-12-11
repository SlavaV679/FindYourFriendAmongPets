using Microsoft.EntityFrameworkCore;
using PetFriend.Accounts.Domain;
using PetFriend.Accounts.Infrastructure.DbContexts;

namespace PetFriend.Accounts.Infrastructure.IdentityManagers;

public class RolePermissionManager(AccountsWriteDbContext accountsWriteDbContext)
{
    public async Task AddRangeIfExistAsync(
        Guid roleId, IEnumerable<string> permissions, CancellationToken cancellationToken = default)
    {
        foreach (var permissionCode in permissions)
        {
            var permission = await accountsWriteDbContext.Permissions.FirstOrDefaultAsync(p => p.Code == permissionCode,
                cancellationToken: cancellationToken);

            if (permission is null)
                throw new ArgumentNullException(nameof(roleId));

            var isPermissionRoleExist = await accountsWriteDbContext.RolePermissions
                .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission!.Id,
                    cancellationToken);

            if (isPermissionRoleExist) continue;
            
            var rolePermission = new RolePermission { RoleId = roleId, PermissionId = permission!.Id };
            await accountsWriteDbContext.RolePermissions.AddAsync(rolePermission, cancellationToken);
        }

        await accountsWriteDbContext.SaveChangesAsync(cancellationToken);
    }
}