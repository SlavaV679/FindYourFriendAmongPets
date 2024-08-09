namespace FindYourFriendAmongPets.Core.Models;

public class Pet
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Type { get; private set; }

    public string Description { get; private set; }

    public string Breed { get; private set; }

    public string Color { get; private set; }

    public string HealphInfo { get; private set; }

    public string Location { get; private set; }

    public double Weight { get; private set; }

    public double Height { get; private set; }

    public string OwnersPhoneNumber { get; private set; }

    public string Neutered { get; private set; }

    public DateOnly DateOfBirth { get; private set; }

    public bool Vaccinated { get; private set; }

    public Status HelpStatus { get; private set; }

    public List<Requisite> Requisites { get; private set; }

    public DateTime DateCreated { get; private set; }
}