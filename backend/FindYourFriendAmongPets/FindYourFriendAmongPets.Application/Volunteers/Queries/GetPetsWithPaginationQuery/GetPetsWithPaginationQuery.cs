namespace FindYourFriendAmongPets.Application.Volunteers.Queries.GetPetsWithPaginationQuery;

public record GetPetsWithPaginationQuery(string? Name, int? PositionFrom, int? PositionTo, int Page, int PageSize);