using FluentValidation;

namespace FindYourFriendAmongPets.Application.Volunteers.Delete;

public class DeleteVolunteerCommandValidator : AbstractValidator<DeleteVolunteerCommand>
{
    public DeleteVolunteerCommandValidator()
    {
        RuleFor(d => d.VolunteerId).NotEmpty();
    }
}