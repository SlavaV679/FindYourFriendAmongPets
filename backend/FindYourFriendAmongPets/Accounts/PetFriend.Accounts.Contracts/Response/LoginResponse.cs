namespace PetFriend.Accounts.Contracts.Response;

public record LoginResponse(string AccessToken, Guid RefreshToken);