using PetFriend.Accounts.Application;
using PetFriend.Accounts.Domain.TypeAccounts;
using PetFriend.Accounts.Infrastructure.DbContexts;

namespace PetFriend.Accounts.Infrastructure.IdentityManagers;

public class ParticipantAccountManager(AccountsWriteDbContext context) : IParticipantAccountManager
{
    public async Task CreateParticipantAccountAsync(
        ParticipantAccount participantAccount, CancellationToken cancellationToken = default)
    {
        await context.Participants.AddAsync(participantAccount, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}