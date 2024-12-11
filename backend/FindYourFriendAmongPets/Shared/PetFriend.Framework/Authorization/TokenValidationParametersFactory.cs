using System.Text;
using Microsoft.IdentityModel.Tokens;
using PetFriend.Framework.Options;

namespace PetFriend.Framework.Authorization;

public static class TokenValidationParametersFactory
{
    public static TokenValidationParameters CreateTokenValidation(JwtOptions jwtOptions, bool validateLifeTime) =>
        new()
        {
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = validateLifeTime,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };

    public static TokenValidationParameters CreateWithoutLifetime(JwtOptions jwtOptions) =>
        CreateTokenValidation(jwtOptions, false);

    public static TokenValidationParameters CreateWithLifetime(JwtOptions jwtOptions) =>
        CreateTokenValidation(jwtOptions, true);
}