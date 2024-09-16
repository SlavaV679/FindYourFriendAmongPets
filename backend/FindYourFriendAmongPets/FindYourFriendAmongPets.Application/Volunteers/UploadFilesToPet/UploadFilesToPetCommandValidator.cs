using FindYourFriendAmongPets.Application.Validation;
using FindYourFriendAmongPets.Core.Shared;
using FluentValidation;

namespace FindYourFriendAmongPets.Application.Volunteers.UploadFilesToPet;

public class UploadFilesToPetCommandValidator: AbstractValidator<UploadFilesToPetCommand>
{
    public UploadFilesToPetCommandValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleForEach(u => u.Files).SetValidator(new UploadFileDtoValidator());
    }
}