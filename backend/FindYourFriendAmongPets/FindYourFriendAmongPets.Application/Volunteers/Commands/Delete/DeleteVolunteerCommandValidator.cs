using FluentValidation;

namespace FindYourFriendAmongPets.Application.Volunteers.Commands.Delete;

public class DeleteVolunteerCommandValidator : AbstractValidator<DeleteVolunteerCommand>
{
    public DeleteVolunteerCommandValidator()
    {
        RuleFor(d => d.VolunteerId).NotEmpty();
    }
}