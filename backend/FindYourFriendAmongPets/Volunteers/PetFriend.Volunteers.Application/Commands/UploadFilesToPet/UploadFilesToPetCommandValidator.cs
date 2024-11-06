using FluentValidation;
using PetFriend.Core.Validators;
using PetFriend.SharedKernel;

namespace PetFriend.Volunteers.Application.Commands.UploadFilesToPet;

public class UploadFilesToPetCommandValidator: AbstractValidator<UploadFilesToPetCommand>
{
    public UploadFilesToPetCommandValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleForEach(u => u.Files).SetValidator(new UploadFileDtoValidator());
    }
}