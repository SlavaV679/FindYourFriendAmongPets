using FindYourFriendAmongPets.Application.Validation;
using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Shared;
using FluentValidation;

namespace FindYourFriendAmongPets.Application.Volunteers.UpdateMainInfo;

public class UpdateMainInfoDtoValidator : AbstractValidator<UpdateMainInfoDto>
{
    public UpdateMainInfoDtoValidator()
    {
        RuleFor(r => r.FullName)
            .MustBeValueObject(f => FullName.Create(f.FirstName, f.LastName, f.Patronymic));

        RuleFor(r => r.Description).MustBeValueObject(Description.Create);

        RuleFor(c => c.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);

        RuleFor(c => c.ExperienceInYears)
            .GreaterThan(0)
            .WithError(Errors.General.ValueIsInvalid("ExperienceInYears", "ExperienceInYears cannot be less than 0"));
    }
}