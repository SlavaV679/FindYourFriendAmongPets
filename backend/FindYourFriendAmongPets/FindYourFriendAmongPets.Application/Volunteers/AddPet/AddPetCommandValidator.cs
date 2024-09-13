using FindYourFriendAmongPets.Application.Validation;
using FindYourFriendAmongPets.Core.Models;
using FluentValidation;

namespace FindYourFriendAmongPets.Application.Volunteers.AddPet;

public class AddPetCommandValidator : AbstractValidator<AddPetCommand>
{
    public AddPetCommandValidator()
    {
        
        RuleFor(r => r.Name).MinimumLength(2).WithMessage("more then 2");

        RuleFor(r => r.Description).MustBeValueObject(Description.Create);

        RuleFor(r => r.OwnersPhoneNumber).MustBeValueObject(PhoneNumber.Create);

        RuleFor(c => c.Address).MustBeValueObject(
            a => Address.Create(a.City, a.Street, a.Building, a.Description, a.Country));

        RuleForEach(c => c.FileCommands).SetValidator(new CreateFileCommandValidator());
    }
}

public class CreateFileCommandValidator : AbstractValidator<CreateFileCommand>
{
    public CreateFileCommandValidator()
    {
        RuleFor(c => c.FileName).MustBeValueObject(FilePath.Create);
    }
}