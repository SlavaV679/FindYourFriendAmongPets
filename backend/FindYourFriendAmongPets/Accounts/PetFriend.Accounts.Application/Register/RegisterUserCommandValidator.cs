using FluentValidation;
using PetFriend.Core.Validators;
using PetFriend.SharedKernel;
using PetFriend.SharedKernel.ValueObjects;

namespace PetFriend.Accounts.Application.Register;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(r => new { r.Name, r.Surname, r.Patronymic })
            .MustBeValueObject(r => FullName.Create(r.Name, r.Surname, r.Patronymic))
            .When(r => !string.IsNullOrEmpty(r.Name) && !string.IsNullOrEmpty(r.Surname));

        RuleFor(r => r.Email)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("email"))
            .EmailAddress().WithError(Errors.General.ValueIsInvalid("email"));

        RuleFor(r => r.Password)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("password"))
            .MinimumLength(6).WithError(Errors.General.ValueIsInvalid("password"));

        RuleFor(r => r.UserName)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("username"))
            .MinimumLength(3).WithError(Errors.General.ValueIsInvalid("username"))
            .Matches("^[a-zA-Z0-9]*$").WithError(Errors.General.ValueIsInvalid("username"));
    }
}