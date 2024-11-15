using Microsoft.AspNetCore.Identity;
using PetFriend.SharedKernel.ValueObjects;

namespace PetFriend.Accounts.Domain;

public class User : IdentityUser<Guid>
{
    public string PhotoPath { get; init; } = string.Empty;
    public FullName FullName { get; set; }
    //public List<SocialLink> SocialLinks { get; set; } = [];
    public Guid RoleId { get; init; }
    public Role Role { get; init; } = default!;
    
    public static User CreateAdmin(FullName fullName, string userName, string email, Role role)
    {
        return new User
        {
            PhotoPath = "",
            FullName = fullName,
            //SocialLinks = [],
            UserName = userName,
            Email = email,
            Role = role
        };
    }
    
    public static User CreateParticipant(FullName fullName, string userName, string email)//, Role role)
    {
        return new User
        {
            PhotoPath = "",
            FullName = fullName,
            //SocialLinks = [],
            UserName = userName,
            Email = email,
            //Role = role
        };
    }
}