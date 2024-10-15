using Dapper;
using FindYourFriendAmongPets.Application.Abstraction;
using FindYourFriendAmongPets.Application.Database;
using FindYourFriendAmongPets.Application.Dtos;
using FindYourFriendAmongPets.Application.Models;

namespace FindYourFriendAmongPets.Application.Volunteers.Queries.GetVolunteersWithPagination;

public class GetVolunteersWithPaginationHandler
    : IQueryHandler<PagedList<VolunteerDto>, GetFilteredVolunteersWithPaginationQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetVolunteersWithPaginationHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<PagedList<VolunteerDto>> Handle(
        GetFilteredVolunteersWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();

        parameters.Add("@PageSize", query.PageSize);
        parameters.Add("@Offset", (query.Page - 1) * query.PageSize);

        var builder = new SqlBuilder();

        var selectSql = builder.AddTemplate(
            @"SELECT id, description, experience_in_years, first_name, last_name,patronymic, phone_number
                    FROM volunteers
                    /**where**/
                    /**orderby**/
                    LIMIT @PageSize
                    OFFSET @Offset",
            parameters);

        var countSql = builder.AddTemplate(@"select count(*) from volunteers /**where**/");

        if (!string.IsNullOrWhiteSpace(query.FirstName))
            builder.Where($" first_name LIKE @FirstName", new { firstName = "%" + query.FirstName + "%" });

        if (!string.IsNullOrWhiteSpace(query.PhoneNumber))
            builder.Where($" phone_number LIKE @PhoneNumber", new { phoneNumber = "%" + query.PhoneNumber + "%" });

        builder.OrderBy(string.Format(" phone_number {0}", query.OrderDesc ? "desc" : "asc"));
        
        var volunteers = await connection.QueryAsync<VolunteerDto>(
            selectSql.RawSql, selectSql.Parameters);

        var totalCount = await connection.ExecuteScalarAsync<int>(countSql.RawSql, countSql.Parameters);

        return new PagedList<VolunteerDto>()
        {
            Items = volunteers.ToList(),
            TotalCount = totalCount,
            PageSize = query.PageSize,
            Page = query.Page,
        };
    }
}