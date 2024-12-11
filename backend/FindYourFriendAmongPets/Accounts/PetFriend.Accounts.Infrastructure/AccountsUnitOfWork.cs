using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetFriend.Accounts.Application;
using PetFriend.Accounts.Infrastructure.DbContexts;

namespace PetFriend.Accounts.Infrastructure;

public class AccountsUnitOfWork(AccountsWriteDbContext context) :  IAccountsUnitOfWork
{
    public async Task<IDbTransaction> BeginTransaction(CancellationToken token = default)
    {
        var transaction = await context.Database.BeginTransactionAsync(token);
        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken token = default) =>
        await context.SaveChangesAsync(token);
}