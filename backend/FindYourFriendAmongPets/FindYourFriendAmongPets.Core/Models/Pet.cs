using FindYourFriendAmongPets.Core.Shared;

namespace FindYourFriendAmongPets.Core.Models;

public class Pet : Entity<PetId>
{
    private Pet(PetId id) : base(id)
    {
    }

    public string Name { get; private set; }

    public string Species { get; private set; }

    public string Description { get; private set; }

    public string Breed { get; private set; }

    public string Color { get; private set; }

    public string HealthInfo { get; private set; }

    public Address Address { get; private set; }

    public double Weight { get; private set; }

    public double Height { get; private set; }

    public string OwnersPhoneNumber { get; private set; }

    public bool IsNeutered { get; private set; }

    public DateOnly DateOfBirth { get; private set; }

    public bool IsVaccinated { get; private set; }

    public Status HelpStatus { get; private set; }

    public DateTime DateCreated { get; private set; }
    
    public List<PetPhoto> PetPhotos { get; }

    public PetDetails Details { get; private set; }
}