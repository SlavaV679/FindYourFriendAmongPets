namespace PetFriend.Core.Dtos;

public record RequisiteForHelpDto(
    string Name,
    string Description,
    Guid? VolunteerId);