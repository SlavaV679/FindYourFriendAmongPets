using FindYourFriendAmongPets.Application.Validation;
using FindYourFriendAmongPets.Core.Models;
using FluentValidation;

namespace FindYourFriendAmongPets.Application.Volunteers.Commands.Create;

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