using System.Data;
using FindYourFriendAmongPets.Application.Database;
using FindYourFriendAmongPets.DataAccess.DBContexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace FindYourFriendAmongPets.DataAccess;

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