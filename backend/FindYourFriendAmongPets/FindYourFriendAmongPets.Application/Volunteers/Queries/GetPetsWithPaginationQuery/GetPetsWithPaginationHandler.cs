using FindYourFriendAmongPets.Application.Database;
using FindYourFriendAmongPets.Application.Dtos;
using FindYourFriendAmongPets.Application.Extensions;
using FindYourFriendAmongPets.Application.Models;

namespace FindYourFriendAmongPets.Application.Volunteers.Queries.GetPetsWithPaginationQuery;

public class GetPetsWithPaginationHandler
{
    private readonly IReadDbContext _readDbContext;
    public GetPetsWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    public async Task<PagedList<PetDto>> Handle(
        GetPetsWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var petsQuery = _readDbContext.Pets.AsQueryable();
        // будущая фильтрация и сортровка
        return await petsQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}