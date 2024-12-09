namespace PetFriend.Accounts.Infrastructure.Authorization;

public class JwtOptions
{
    public static string JWT = nameof(JWT);
    public string Audience { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string ExpirationInMinutes { get; set; } = string.Empty;
}