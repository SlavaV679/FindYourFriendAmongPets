namespace PetFriend.Core.Dtos;

public record AddressDto(
    string City, 
    string Street, 
    string? Building = null, 
    string? Description = null, 
    string? Country = null);