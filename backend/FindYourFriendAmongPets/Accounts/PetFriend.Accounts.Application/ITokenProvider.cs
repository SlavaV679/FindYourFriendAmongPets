using System.Security.Claims;
using CSharpFunctionalExtensions;
using PetFriend.Accounts.Application.Models;
using PetFriend.Accounts.Domain;
using PetFriend.SharedKernel;

namespace PetFriend.Accounts.Application;

public interface ITokenProvider
{
    JwtTokenResult GenerateAccessToken(User user);
    
    Task<Guid> GenerateRefreshToken(User user, Guid jti, CancellationToken cancellationToken);
    
    public Task<Result<IReadOnlyList<Claim>, Error>> GetUserClaims(string jwtToken);
}