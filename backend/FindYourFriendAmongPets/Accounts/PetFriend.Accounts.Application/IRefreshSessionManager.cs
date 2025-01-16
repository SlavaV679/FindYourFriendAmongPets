using CSharpFunctionalExtensions;
using PetFriend.Accounts.Domain;
using PetFriend.SharedKernel;

namespace PetFriend.Accounts.Application;

public interface IRefreshSessionManager
{
    public Task<Result<RefreshSession, Error>> GetByRefreshTokenAsync(
        Guid refreshToken, CancellationToken cancellationToken = default);

    public void Delete(RefreshSession refreshSession);
}