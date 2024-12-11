using Microsoft.AspNetCore.Identity;

namespace PetFriend.Accounts.Domain;

public class Role : IdentityRole<Guid>
{
    public IEnumerable<User> Users { get; init; }
    public IEnumerable<RolePermission> RolePermissions { get; init; }
}