using FindYourFriendAmongPets.Application.Abstraction;

namespace FindYourFriendAmongPets.Application.Volunteers.Queries.GetVolunteerById;

public record GetVolunteerByIdQuery(
    Guid? Id): IQuery;