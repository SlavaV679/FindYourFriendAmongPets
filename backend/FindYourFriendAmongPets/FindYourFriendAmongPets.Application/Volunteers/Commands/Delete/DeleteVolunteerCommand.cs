using FindYourFriendAmongPets.Application.Abstraction;

namespace FindYourFriendAmongPets.Application.Volunteers.Commands.Delete;

public record DeleteVolunteerCommand(Guid VolunteerId): ICommand;