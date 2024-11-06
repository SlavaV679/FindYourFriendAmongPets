using PetFriend.Core.Abstractions;

namespace PetFriend.Volunteers.Application.Queries.GetVolunteersWithPagination;

public record GetFilteredVolunteersWithPaginationQuery(
    string? FirstName,
    string? PhoneNumber,
    int Page,
    int PageSize,
    bool OrderDesc = false): IQuery;