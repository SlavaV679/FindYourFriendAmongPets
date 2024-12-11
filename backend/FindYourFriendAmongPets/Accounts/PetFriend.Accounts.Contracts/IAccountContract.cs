namespace PetFriend.Accounts.Contracts;

public interface IAccountContract
{
    public Task<IEnumerable<string>?> GetPermissionsUserById(
        Guid userId, CancellationToken cancellationToken = default);
}