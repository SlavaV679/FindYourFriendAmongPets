using FindYourFriendAmongPets.Application.Volunteers.Queries.GetVolunteersWithPagination;

namespace FindYourFriendAmongPets.API.Controllers.Queries.Requests;

public record GetFilteredVolunteersWithPaginationRequest(
    string? FirstName,
    string? PhoneNumber,
    int Page,
    int PageSize,
    bool OrderDesc = false)
{
    public GetFilteredVolunteersWithPaginationQuery ToQuery() => new(FirstName, PhoneNumber, Page, PageSize, OrderDesc);
}