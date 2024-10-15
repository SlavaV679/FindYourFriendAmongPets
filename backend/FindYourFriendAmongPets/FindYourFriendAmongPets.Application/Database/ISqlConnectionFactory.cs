using System.Data;

namespace FindYourFriendAmongPets.Application.Database;

public interface ISqlConnectionFactory
{
    IDbConnection Create();
}