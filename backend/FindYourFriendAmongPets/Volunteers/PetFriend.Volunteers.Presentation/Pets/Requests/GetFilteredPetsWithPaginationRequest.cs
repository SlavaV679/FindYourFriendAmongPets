using PetFriend.Volunteers.Application.Queries.GetPetsWithPaginationQuery;

namespace PetFriend.Volunteers.Presentation.Pets.Requests;

public record GetFilteredPetsWithPaginationRequest(string? Name, int? PositionFrom, int? PositionTo, int Page, int PageSize)
{
    public GetFilteredPetsWithPaginationQuery ToQuery() => new(Name, PositionFrom, PositionTo, Page, PageSize);
}