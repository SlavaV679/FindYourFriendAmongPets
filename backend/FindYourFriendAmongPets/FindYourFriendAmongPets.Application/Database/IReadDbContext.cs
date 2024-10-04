using FindYourFriendAmongPets.Application.Dtos;
using Microsoft.EntityFrameworkCore;

namespace FindYourFriendAmongPets.Application.Database;

public interface IReadDbContext
{
    DbSet<VolunteerDto> Volunteers { get; }
    DbSet<PetDto> Pets { get; }
}