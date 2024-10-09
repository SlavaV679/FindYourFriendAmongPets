using FindYourFriendAmongPets.Application.Abstraction;

namespace FindYourFriendAmongPets.Application.Volunteers.Queries.GetPetsWithPaginationQuery;

public record GetFilteredPetsWithPaginationQuery(
    string? Name,
    int? PositionFrom,
    int? PositionTo,
    int Page,
    int PageSize): IQuery;