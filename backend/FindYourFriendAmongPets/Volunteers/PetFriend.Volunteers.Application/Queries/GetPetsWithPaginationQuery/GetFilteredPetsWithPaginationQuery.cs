using PetFriend.Core.Abstractions;

namespace PetFriend.Volunteers.Application.Queries.GetPetsWithPaginationQuery;

public record GetFilteredPetsWithPaginationQuery(
    string? Name,
    int? PositionFrom,
    int? PositionTo,
    int Page,
    int PageSize): IQuery;