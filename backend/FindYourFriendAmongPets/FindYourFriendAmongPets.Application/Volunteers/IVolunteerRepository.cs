using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Core.Models;

namespace FindYourFriendAmongPets.Application.Volunteers;

public interface IVolunteerRepository
{
    Task<VolunteerId> Add(Volunteer volunteer, CancellationToken cancellationToken = default);

    Task<Result<Volunteer, string>> GetById(VolunteerId id, CancellationToken cancellationToken = default);
}