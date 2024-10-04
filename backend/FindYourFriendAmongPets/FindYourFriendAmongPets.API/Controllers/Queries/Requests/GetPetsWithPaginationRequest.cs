using FindYourFriendAmongPets.Application.Volunteers.Queries.GetPetsWithPaginationQuery;

namespace FindYourFriendAmongPets.API.Controllers.Queries.Requests;

public record GetPetsWithPaginationRequest(int Page, int PageSize)
{
    public GetPetsWithPaginationQuery ToQuery() => new(Page, PageSize);
}