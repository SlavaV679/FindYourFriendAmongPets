using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using PetFriend.Accounts.Contracts.Response;
using PetFriend.Accounts.Domain;
using PetFriend.Core.Abstractions;
using PetFriend.Core.Extensions;
using PetFriend.SharedKernel;

namespace PetFriend.Accounts.Application.Login;

public class LoginHandler(
    UserManager<User> userManager,
    ITokenProvider tokenProvider,
    IValidator<LoginUserCommand> validator) : ICommandHandler<LoginResponse, LoginUserCommand>
{
    public async Task<Result<LoginResponse, ErrorList>> Handle(
        LoginUserCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToList();

        var existsUser = await userManager.FindByEmailAsync(command.Email);
        if (existsUser is null)
            return Errors.User.InvalidCredentials().ToErrorList();

        var passwordCorrect = await userManager.CheckPasswordAsync(existsUser, command.Password);
        if (!passwordCorrect)
            return Errors.User.InvalidCredentials().ToErrorList();

        var accessToken = await tokenProvider.GenerateAccessToken(existsUser, cancellationToken);
        var refreshToken = await tokenProvider.GenerateRefreshToken(existsUser, accessToken.Jti, cancellationToken);
        return new LoginResponse(accessToken.AccessToken, refreshToken);
    }
}