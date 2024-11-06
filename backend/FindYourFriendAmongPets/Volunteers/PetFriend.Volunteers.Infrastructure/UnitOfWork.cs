using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetFriend.Volunteers.Application.Database;
using PetFriend.Volunteers.Infrastructure.DbContexts;

namespace PetFriend.Volunteers.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly PetFamilyWriteDbContext _writeDbContext;

    public UnitOfWork(PetFamilyWriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }

    public async Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await _writeDbContext.Database.BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await _writeDbContext.SaveChangesAsync(cancellationToken);
    }
}