using PetFriend.Volunteers.Application.Queries.GetVolunteersWithPagination;

namespace PetFriend.Volunteers.Presentation.Volunteers.Requests;

public record GetFilteredVolunteersWithPaginationRequest(
    string? FirstName,
    string? PhoneNumber,
    int Page,
    int PageSize,
    bool OrderDesc = false)
{
    public GetFilteredVolunteersWithPaginationQuery ToQuery() => new(FirstName, PhoneNumber, Page, PageSize, OrderDesc);
}