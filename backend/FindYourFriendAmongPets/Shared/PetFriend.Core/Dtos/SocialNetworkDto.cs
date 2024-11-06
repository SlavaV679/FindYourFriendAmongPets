namespace PetFriend.Core.Dtos;

public record SocialNetworkDto(
    string Title,
    string Link,
    Guid? VolunteerId);