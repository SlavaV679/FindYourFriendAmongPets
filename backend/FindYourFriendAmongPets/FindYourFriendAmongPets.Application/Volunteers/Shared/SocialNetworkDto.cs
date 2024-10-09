namespace FindYourFriendAmongPets.Application.Volunteers.Shared;

public record SocialNetworkDto(
    string Title,
    string Link,
    Guid? VolunteerId);