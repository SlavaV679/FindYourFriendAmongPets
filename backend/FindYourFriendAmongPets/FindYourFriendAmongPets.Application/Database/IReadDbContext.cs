using FindYourFriendAmongPets.Application.Dtos;
using Microsoft.EntityFrameworkCore;

namespace FindYourFriendAmongPets.Application.Database;

public interface IReadDbContext
{
    IQueryable<VolunteerDto> Volunteers { get; }
    IQueryable<PetDto> Pets { get; }
}