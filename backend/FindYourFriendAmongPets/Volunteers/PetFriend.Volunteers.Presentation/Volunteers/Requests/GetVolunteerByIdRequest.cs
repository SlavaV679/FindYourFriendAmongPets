using PetFriend.Volunteers.Application.Queries.GetVolunteerById;

namespace PetFriend.Volunteers.Presentation.Volunteers.Requests;

public record GetVolunteerByIdRequest(
    Guid? Id)
{
    public GetVolunteerByIdQuery ToQuery() => new(Id);
}