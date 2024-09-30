namespace FindYourFriendAmongPets.Application.Volunteers.Commands.AddPet;

public record AddressDto(string City, string Street, string? Building = null, string? Description = null, string? Country = null);