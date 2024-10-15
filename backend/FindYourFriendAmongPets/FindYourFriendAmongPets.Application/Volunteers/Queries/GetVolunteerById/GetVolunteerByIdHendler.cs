using Dapper;
using FindYourFriendAmongPets.Application.Abstraction;
using FindYourFriendAmongPets.Application.Database;
using FindYourFriendAmongPets.Application.Dtos;

namespace FindYourFriendAmongPets.Application.Volunteers.Queries.GetVolunteerById;

public class GetVolunteerByIdHandler
    : IQueryHandler<VolunteerDto, GetVolunteerByIdQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetVolunteerByIdHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<VolunteerDto?> Handle(
        GetVolunteerByIdQuery query,
        CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();

        var builder = new SqlBuilder();

        var selectSql = builder.AddTemplate(
            @"SELECT id, description, experience_in_years, first_name, last_name,patronymic, phone_number
                    FROM volunteers
                    /**where**/",
            parameters);

        builder.Where($" id = @volunteerId", new { volunteerId = query.Id });

        var volunteer = await connection.QueryFirstOrDefaultAsync<VolunteerDto>(
            selectSql.RawSql, selectSql.Parameters);

        return volunteer;
    }
}