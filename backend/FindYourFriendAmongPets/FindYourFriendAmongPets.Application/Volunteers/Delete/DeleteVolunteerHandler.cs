﻿using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Shared;
using Microsoft.Extensions.Logging;

namespace FindYourFriendAmongPets.Application.Volunteers.Delete;

public class DeleteVolunteerHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<DeleteVolunteerHandler> _logger;

    public DeleteVolunteerHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<DeleteVolunteerHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        DeleteVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var id = VolunteerId.Create(request.VolunteerId);

        var moduleResult = await _volunteerRepository.GetById(id, cancellationToken);
        if (moduleResult.IsFailure)
            return moduleResult.Error;

        var result = await _volunteerRepository.Delete(moduleResult.Value, cancellationToken);

        _logger.LogInformation("Updated deleted with id {volunteerId}", request.VolunteerId);

        return result.Value;
    }
}