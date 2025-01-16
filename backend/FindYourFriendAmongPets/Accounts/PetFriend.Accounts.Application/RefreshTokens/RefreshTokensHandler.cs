using System.Security.Claims;
using CSharpFunctionalExtensions;
using FluentValidation;
using PetFriend.Accounts.Contracts.Response;
using PetFriend.Accounts.Domain;
using PetFriend.Core.Abstractions;
using PetFriend.Core.Extensions;
using PetFriend.Core.Models;
using PetFriend.SharedKernel;

namespace PetFriend.Accounts.Application.RefreshTokens;

public class RefreshTokensHandler(
    IValidator<RefreshTokensCommand> validator,
    ITokenProvider tokenProvider,
    IRefreshSessionManager refreshSessionManager) : ICommandHandler<LoginResponse, RefreshTokensCommand>
{
    public async Task<Result<LoginResponse, ErrorList>> Handle(
        RefreshTokensCommand command, CancellationToken cancellationToken = default)
    {
        var oldRefreshSession = await refreshSessionManager
            .GetByRefreshTokenAsync(command.RefreshToken, cancellationToken);

        if (oldRefreshSession.IsFailure)
            return oldRefreshSession.Error.ToErrorList();

        if (oldRefreshSession.Value.ExpiredAt < DateTime.UtcNow)
        {
            return Errors.Token.ExpiredToken().ToErrorList();
        }

        refreshSessionManager.Delete(oldRefreshSession.Value);

        var accessToken = await tokenProvider.GenerateAccessToken(oldRefreshSession.Value.User, cancellationToken);

        var refreshToken = await tokenProvider.GenerateRefreshToken(
            oldRefreshSession.Value.User,
            accessToken.Jti,
            cancellationToken);

        return new LoginResponse(
            accessToken.AccessToken,
            refreshToken,
            oldRefreshSession.Value.User.Id,
            oldRefreshSession.Value.User.Email);
    }
}