using FindYourFriendAmongPets.Application.Abstraction;

namespace FindYourFriendAmongPets.Application.Volunteers.Queries.GetVolunteersWithPagination;

public record GetFilteredVolunteersWithPaginationQuery(
    string? FirstName,
    string? PhoneNumber,
    int Page,
    int PageSize,
    bool OrderDesc = false): IQuery;