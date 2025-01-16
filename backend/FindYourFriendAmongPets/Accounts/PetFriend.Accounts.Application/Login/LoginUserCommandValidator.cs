using FluentValidation;
using PetFriend.Core.Validators;
using PetFriend.SharedKernel;

namespace PetFriend.Accounts.Application.Login;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(l => l.Email)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("email"))
            .EmailAddress()
            .WithError(Errors.General.ValueIsInvalid("email"));

        RuleFor(l => l.Password)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("password"))
            .MinimumLength(4)
            .WithError(Errors.General.ValueIsInvalid("password"));
    }
}