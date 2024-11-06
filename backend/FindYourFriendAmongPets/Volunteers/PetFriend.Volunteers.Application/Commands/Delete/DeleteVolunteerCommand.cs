using PetFriend.Core.Abstractions;

namespace PetFriend.Volunteers.Application.Commands.Delete;

public record DeleteVolunteerCommand(Guid VolunteerId): ICommand;