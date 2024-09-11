using System.Data;
using FindYourFriendAmongPets.Application.Database;
using Microsoft.EntityFrameworkCore.Storage;

namespace FindYourFriendAmongPets.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly PetFamilyDbContext _dbContext;

    public UnitOfWork(PetFamilyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}