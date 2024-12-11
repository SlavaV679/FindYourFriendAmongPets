using PetFriend.Accounts.Domain.TypeAccounts;

namespace PetFriend.Accounts.Application;

public interface IVolunteerAccountManager
{
    public Task CreateVolunteerAccountAsync(VolunteerAccount volunteer, CancellationToken cancellationToken = default);
    public Task<VolunteerAccount?> GetVolunteerAccountByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    public Task UpdateAsync(VolunteerAccount volunteer, CancellationToken cancellationToken = default);
}