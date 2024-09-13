using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Application.Volunteers;
using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Shared;
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

        // await _dbContext.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }

    public async Task<Result<Volunteer, Error>> GetById(VolunteerId id, CancellationToken cancellationToken = default) //VolunteerId volunteerId)
    {
        var volunteer = await _dbContext.Volunteers
            .Include(m => m.Pets)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (volunteer is null)
            return Errors.General.NotFound(id.Value); // "Volunteer not found";

        return volunteer;
    }

    public async Task<Result<Volunteer, Error>> GetByPhoneNumber(
        PhoneNumber phoneNumber,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _dbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.PhoneNumber == phoneNumber, cancellationToken);

        if (volunteer == null)
            return Errors.General.NotFound();

        return volunteer;
    }

    public VolunteerId Delete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _dbContext.Volunteers.Remove(volunteer);

        return volunteer.Id;
    }

    public Guid Save(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _dbContext.Volunteers.Attach(volunteer);

        return volunteer.Id.Value;
    }
}