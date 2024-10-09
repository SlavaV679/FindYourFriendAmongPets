namespace FindYourFriendAmongPets.Application.Volunteers.Shared;

public record RequisiteForHelpDto(
    string Name,
    string Description,
    Guid? VolunteerId);