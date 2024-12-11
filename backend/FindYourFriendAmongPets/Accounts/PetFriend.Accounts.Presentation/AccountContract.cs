using PetFriend.Accounts.Contracts;
using PetFriend.Accounts.Infrastructure.IdentityManagers;

namespace PetFriend.Accounts.Presentation;

public class AccountContract(PermissionManager permissionManager) : IAccountContract
{
    public async Task<IEnumerable<string>?> GetPermissionsUserById(
        Guid userId, CancellationToken cancellationToken = default)
    {
        return await permissionManager.GetPermissionsByUserId(userId, cancellationToken);
    }
}