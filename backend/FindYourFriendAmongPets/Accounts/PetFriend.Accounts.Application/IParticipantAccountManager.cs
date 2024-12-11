using PetFriend.Accounts.Domain.TypeAccounts;

namespace PetFriend.Accounts.Application;

public interface IParticipantAccountManager
{
    public Task CreateParticipantAccountAsync(
        ParticipantAccount participantAccount, CancellationToken cancellationToken = default);
}