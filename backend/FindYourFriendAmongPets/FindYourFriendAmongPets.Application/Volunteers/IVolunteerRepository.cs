﻿using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Shared;

namespace FindYourFriendAmongPets.Application.Volunteers;

public interface IVolunteerRepository
{
    Task<VolunteerId> Add(Volunteer volunteer, CancellationToken cancellationToken = default);

    Task<Result<Volunteer, Error>> GetById(VolunteerId id, CancellationToken cancellationToken = default);

    Task<Result<Volunteer, Error>> GetByPhoneNumber(
        PhoneNumber phoneNumber,
        CancellationToken cancellationToken = default);

    Guid Save(Volunteer volunteer, CancellationToken cancellationToken = default);
  
    VolunteerId Delete(Volunteer volunteer, CancellationToken cancellationToken = default);
}