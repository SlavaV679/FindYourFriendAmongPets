using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFriend.Accounts.Application;
using PetFriend.Accounts.Domain;
using PetFriend.Accounts.Infrastructure.DbContexts;
using PetFriend.SharedKernel;

namespace PetFriend.Accounts.Infrastructure.IdentityManagers;

public class RefreshSessionManager(
    AccountsWriteDbContext context) : IRefreshSessionManager
{
    public async Task<Result<RefreshSession, Error>> GetByRefreshTokenAsync(
        Guid refreshToken, CancellationToken cancellationToken = default)
    {
        var refreshSession = await context.RefreshSessions
            .Include(s=> s.User)
            .FirstOrDefaultAsync(s => s.RefreshToken == refreshToken, cancellationToken);
        if (refreshSession is null)
            return Errors.General.NotFound();
        
        return refreshSession;
    }

    public void Delete(RefreshSession refreshSession)
    {
        context.RefreshSessions.Remove(refreshSession);
    }
}