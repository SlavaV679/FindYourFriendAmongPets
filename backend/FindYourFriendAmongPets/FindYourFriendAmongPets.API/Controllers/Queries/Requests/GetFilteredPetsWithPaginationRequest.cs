using FindYourFriendAmongPets.Application.Volunteers.Queries.GetPetsWithPaginationQuery;

namespace FindYourFriendAmongPets.API.Controllers.Queries.Requests;

public record GetFilteredPetsWithPaginationRequest(string? Name, int? PositionFrom, int? PositionTo, int Page, int PageSize)
{
    public GetFilteredPetsWithPaginationQuery ToQuery() => new(Name, PositionFrom, PositionTo, Page, PageSize);
}