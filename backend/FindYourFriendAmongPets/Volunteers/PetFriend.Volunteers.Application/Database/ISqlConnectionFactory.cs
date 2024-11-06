using System.Data;

namespace PetFriend.Volunteers.Application.Database;

public interface ISqlConnectionFactory
{
    IDbConnection Create();
}