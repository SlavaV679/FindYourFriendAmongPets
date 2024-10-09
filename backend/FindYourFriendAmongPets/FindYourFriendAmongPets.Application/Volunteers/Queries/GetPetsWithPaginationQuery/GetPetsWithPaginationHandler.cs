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
        var petsQuery = _readDbContext.Pets;//.AsQueryable();
        
        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Name),
            i => i.Name.Contains(query.Name!));

        petsQuery = petsQuery.WhereIf(
            query.PositionTo != null,
            i => i.Position <= query.PositionTo!.Value);
        
        petsQuery = petsQuery.WhereIf(
            query.PositionFrom != null,
            i => i.Position >= query.PositionFrom!.Value);
        
        return await petsQuery
            .OrderBy(i => i.Position)
            .ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}