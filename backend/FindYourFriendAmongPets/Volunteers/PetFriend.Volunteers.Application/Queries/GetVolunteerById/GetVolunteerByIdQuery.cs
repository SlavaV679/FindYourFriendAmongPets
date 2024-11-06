using PetFriend.Core.Abstractions;

namespace PetFriend.Volunteers.Application.Queries.GetVolunteerById;

public record GetVolunteerByIdQuery(
    Guid? Id): IQuery;