using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Application.Volunteers;
using FindYourFriendAmongPets.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FindYourFriendAmongPets.DataAccess.Repositories;

public class VolunteerRepository : IVolunteerRepository
{
    private readonly PetFamilyDbContext _dbContext;

    public VolunteerRepository(PetFamilyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<VolunteerId> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }

    public async Task<Result<Volunteer, string>> GetById(VolunteerId id, CancellationToken cancellationToken = default) //VolunteerId volunteerId)
    {
        var volunteer = await _dbContext.Volunteers
            .Include(m => m.Pets)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (volunteer is null)
            return "Volunteer not found";

        return volunteer;
    }
}