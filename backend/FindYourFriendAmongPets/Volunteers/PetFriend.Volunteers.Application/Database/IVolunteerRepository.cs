using CSharpFunctionalExtensions;
using PetFriend.SharedKernel;
using PetFriend.SharedKernel.ValueObjects.Ids;
using PetFriend.Volunteers.Domain;
using PetFriend.Volunteers.Domain.ValueObject;

namespace PetFriend.Volunteers.Application.Database;

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