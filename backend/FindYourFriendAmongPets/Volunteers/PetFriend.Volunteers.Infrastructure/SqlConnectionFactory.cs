using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PetFriend.SharedKernel;
using PetFriend.Volunteers.Application.Database;

namespace PetFriend.Volunteers.Infrastructure;

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