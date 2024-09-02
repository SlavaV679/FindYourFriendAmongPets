using FindYourFriendAmongPets.Application.Validation;
using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Shared;
using FluentValidation;

namespace FindYourFriendAmongPets.Application.Volunteers.Create;

public class CreateVolunteerRequestValidator : AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerRequestValidator()
    {
        RuleFor(r => r.FullName)
            .MustBeValueObject(f => FullName.Create(f.FirstName,f.LastName,f.Patronymic));
        
        RuleFor(c => c.Description)
            .MustBeValueObject(Description.Create);
        
        RuleFor(c => c.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);
        
        RuleFor(c => c.ExperienceInYears)
            .GreaterThan(0)
            .WithError(Errors.General.ValueIsInvalid("ExperienceInYears", "ExperienceInYears cannot be less than 0"));
    }
}