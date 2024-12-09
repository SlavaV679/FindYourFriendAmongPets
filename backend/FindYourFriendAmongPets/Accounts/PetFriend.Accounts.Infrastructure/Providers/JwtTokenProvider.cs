using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFriend.Accounts.Application;
using PetFriend.Accounts.Application.Models;
using PetFriend.Accounts.Domain;
using PetFriend.Accounts.Infrastructure.Authorization;
using PetFriend.Accounts.Infrastructure.DbContexts;
using PetFriend.Framework.Authorization;
using PetFriend.SharedKernel;

namespace PetFriend.Accounts.Infrastructure.Providers;

public class JwtTokenProvider(IOptions<JwtOptions> options, AccountsWriteDbContext context) : ITokenProvider
{
    public JwtTokenResult GenerateAccessToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var jti = Guid.NewGuid();

        Claim[] claims =
        [
            new(CustomClaims.Id, user.Id.ToString()),
            new(CustomClaims.Email, user.Email ?? ""),
            new(CustomClaims.Jti, jti.ToString()),
            new(CustomClaims.UserName, user.UserName ?? "")
        ];

        var jwtToken = new JwtSecurityToken(issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(options.Value.ExpirationInMinutes)),
            signingCredentials: signingCredentials,
            claims: claims);

        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return new JwtTokenResult(token, jti);
    }

    public async Task<Result<IReadOnlyList<Claim>, Error>> GetUserClaims(string jwtToken)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var validationParameters = TokenValidationParametersFactory.CreateWithoutLifetime(options.Value);
        var validationResult = await jwtHandler.ValidateTokenAsync(jwtToken, validationParameters);
        if (validationResult.IsValid == false)
            return Errors.Token.InvalidToken();

        return validationResult.ClaimsIdentity.Claims.ToList();
    }

    public async Task<Guid> GenerateRefreshToken(User user, Guid jti, CancellationToken cancellationToken)
    {
        var isUserExists = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken);
        // if (isUserExists == null)        //TODO обработать если ползьватель не найден
        
        var refreshSession = new RefreshSession()
        {
            RefreshToken = Guid.NewGuid(),
            UserId = user.Id,
            User = user,
            CreatedAt = DateTime.UtcNow,
            ExpiredAt = DateTime.UtcNow.AddDays(60),
            Jti = jti
        };

        context.RefreshSessions.Add(refreshSession);
        await context.SaveChangesAsync(cancellationToken);

        return refreshSession.RefreshToken;
    }
}