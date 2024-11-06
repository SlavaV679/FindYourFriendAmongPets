namespace PetFriend.Volunteers.Domain.Entities;

public class SocialNetwork
{
    public SocialNetwork(Guid id, string title, string link)
    {
        Id = id;
        Title = title;
        Link = link;
    }

    public Guid Id { get; private set; }
    
    public string Title { get; private set; }
    
    public string Link { get; private set; }
}