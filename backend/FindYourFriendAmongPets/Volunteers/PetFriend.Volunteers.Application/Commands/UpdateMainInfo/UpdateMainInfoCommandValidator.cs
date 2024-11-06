using FluentValidation;
using PetFriend.Core.Validators;
using PetFriend.SharedKernel;

namespace PetFriend.Volunteers.Application.Commands.UpdateMainInfo;

public class UpdateMainInfoCommandValidator : AbstractValidator<UpdateMainInfoCommand>
{
    public UpdateMainInfoCommandValidator()
    {
        RuleFor(r => r.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}