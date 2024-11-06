using FluentValidation;
using PetFriend.Core.Validators;
using PetFriend.Volunteers.Domain.ValueObject;

namespace PetFriend.Volunteers.Application.Commands.Create;

public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerCommandValidator()
    {
        RuleFor(r => r.FullName)
            .MustBeValueObject(f => FullName.Create(f.FirstName,f.LastName,f.Patronymic));
        
        RuleFor(c => c.Description)
            .MustBeValueObject(Description.Create);
        
        RuleFor(c => c.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);

        RuleFor(c => c.ExperienceInYears)
            .GreaterThan(0);
            //.WithError(Errors.General.ValueIsInvalid("ExperienceInYears", "ExperienceInYears cannot be less than 0"));
    }
}