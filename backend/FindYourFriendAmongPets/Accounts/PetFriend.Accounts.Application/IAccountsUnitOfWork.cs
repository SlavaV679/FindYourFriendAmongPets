using System.Data;

namespace PetFriend.Accounts.Application;

public interface IAccountsUnitOfWork
{
    public Task<IDbTransaction> BeginTransaction(CancellationToken token = default);

    public Task SaveChanges(CancellationToken token = default);
}