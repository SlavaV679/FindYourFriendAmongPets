using FluentValidation;

namespace FindYourFriendAmongPets.Application.Volunteers.Delete;

public class DeleteVolunteerRequestValidator : AbstractValidator<DeleteVolunteerRequest>
{
    public DeleteVolunteerRequestValidator()
    {
        RuleFor(d => d.VolunteerId).NotEmpty();
    }
}