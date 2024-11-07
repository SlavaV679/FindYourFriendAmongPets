namespace PetFriend.Accounts.Domain;

public class Permission
{
    public Guid Id { get; init; }
    public string Code { get; init; }
    public IEnumerable<RolePermission> RolePermissions { get; init; } = [];
}