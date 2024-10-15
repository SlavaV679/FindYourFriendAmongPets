using FindYourFriendAmongPets.Application.Volunteers.Queries.GetVolunteerById;

namespace FindYourFriendAmongPets.API.Controllers.Queries.Requests;

public record GetVolunteerByIdRequest(
    Guid? Id)
{
    public GetVolunteerByIdQuery ToQuery() => new(Id);
}