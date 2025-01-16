using FluentValidation;

namespace PetFriend.Accounts.Application.RefreshTokens;

public class RefreshTokensValidator : AbstractValidator<RefreshTokensCommand>
{
    public RefreshTokensValidator()
    {
        RuleFor(r => r.RefreshToken).NotEmpty();
    }
}