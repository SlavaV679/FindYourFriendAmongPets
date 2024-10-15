using System.Data;
using FindYourFriendAmongPets.Application.Database;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace FindYourFriendAmongPets.DataAccess;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly IConfiguration _configuration;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection Create() =>
        new NpgsqlConnection(_configuration.GetConnectionString(Constants.PET_FAMILY_DATABASE));
}