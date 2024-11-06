using PetFriend.Core.Dtos;

namespace PetFriend.Volunteers.Application.Database;

public interface IReadDbContext
{
    IQueryable<VolunteerDto> Volunteers { get; }
    IQueryable<PetDto> Pets { get; }
}