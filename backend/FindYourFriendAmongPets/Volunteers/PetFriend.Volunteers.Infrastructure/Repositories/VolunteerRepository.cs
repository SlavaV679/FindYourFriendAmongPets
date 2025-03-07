﻿using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFriend.SharedKernel;
using PetFriend.SharedKernel.ValueObjects.Ids;
using PetFriend.Volunteers.Application.Database;
using PetFriend.Volunteers.Domain;
using PetFriend.Volunteers.Domain.ValueObject;
using PetFriend.Volunteers.Infrastructure.DbContexts;

namespace PetFriend.Volunteers.Infrastructure.Repositories;

public class VolunteerRepository : IVolunteerRepository
{
    private readonly PetFamilyWriteDbContext _writeDbContext;

    public VolunteerRepository(PetFamilyWriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }

    public async Task<VolunteerId> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _writeDbContext.Volunteers.AddAsync(volunteer, cancellationToken);

        // await _writeDbContext.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }

    public async Task<Result<Volunteer, Error>> GetById(VolunteerId id, CancellationToken cancellationToken = default) //VolunteerId volunteerId)
    {
        var volunteer = await _writeDbContext.Volunteers
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
        var volunteer = await _writeDbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.PhoneNumber == phoneNumber, cancellationToken);

        if (volunteer == null)
            return Errors.General.NotFound();

        return volunteer;
    }

    public VolunteerId Delete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _writeDbContext.Volunteers.Remove(volunteer);

        return volunteer.Id;
    }

    public Guid Save(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _writeDbContext.Volunteers.Attach(volunteer);

        return volunteer.Id.Value;
    }
}