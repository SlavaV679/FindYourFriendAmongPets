using FindYourFriendAmongPets.Application.Dtos;
using FindYourFriendAmongPets.Core.Shared;
using FluentValidation;

namespace FindYourFriendAmongPets.Application.Validation;

public class UploadFileDtoValidator : AbstractValidator<UploadFileDto>
{
    public UploadFileDtoValidator()
    {
        RuleFor(u => u.FileName).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.Content).Must(c => c.Length < 5000000);
    }
}