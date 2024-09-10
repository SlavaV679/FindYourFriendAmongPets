using FindYourFriendAmongPets.Application.Validation;
using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Shared;
using FluentValidation;

namespace FindYourFriendAmongPets.API.Contracts;

public class AddPetRequestValidator : AbstractValidator<AddPetRequest>
{
    public AddPetRequestValidator()
    {
        //RuleFor(r => r.Name).MinimumLength(30).WithMessage("more then 2");

        RuleFor(r => r.Description).MustBeValueObject(Description.Create);

        RuleFor(r => r.OwnersPhoneNumber).MustBeValueObject(PhoneNumber.Create);

        RuleFor(r => r.Address)
            .MustBeValueObject(a => Address.Create(a.City, a.Street, a.Building, a.Description, a.Country));
    }
}