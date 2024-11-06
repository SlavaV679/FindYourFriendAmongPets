using System.Text.Json;
using Dapper;
using PetFriend.Core.Abstractions;
using PetFriend.Core.Dtos;
using PetFriend.Core.Extensions;
using PetFriend.Core.Models;
using PetFriend.Volunteers.Application.Database;

namespace PetFriend.Volunteers.Application.Queries.GetPetsWithPaginationQuery;

public class GetPetsWithPaginationHandler: IQueryHandler<PagedList<PetDto>, GetFilteredPetsWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;
    
    public GetPetsWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    
    public async Task<PagedList<PetDto>> Handle(
        GetFilteredPetsWithPaginationQuery query,
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

public class GetPetsWithPaginationHandlerDapper
    : IQueryHandler<PagedList<PetDto>, GetFilteredPetsWithPaginationQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetPetsWithPaginationHandlerDapper(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<PagedList<PetDto>> Handle(
        GetFilteredPetsWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.Create();
        var parameters = new DynamicParameters();
        var totalCount = await connection.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM pets");
        var sql = """
                  SELECT id, name, position, pet_files FROM pets
                  ORDER BY position LIMIT @PageSize OFFSET @Offset
                  """;

        parameters.Add("@PageSize", query.PageSize);
        parameters.Add("@Offset", (query.Page - 1) * query.PageSize);
        var pets = await connection.QueryAsync<PetDto, string, PetDto>(
            sql,
            (pet, jsonFiles) =>
            {
                var files = JsonSerializer.Deserialize<PetFileDto[]>(jsonFiles) ?? [];

                pet.PetFiles = files;
                return pet;
            },
            splitOn: "pet_files",
            param: parameters);
        
        return new PagedList<PetDto>()
        {
            Items = pets.ToList(),
            TotalCount = totalCount,
            PageSize = query.PageSize,
            Page = query.Page,
        };
    }
}