namespace PetFriend.Core.Dtos;

public class PetDto
{
    public Guid Id { get; init; }
    public Guid VolunteerId { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid PetSpecies { get; init; }
    public string Description { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public string HealthInfo { get; init; } = string.Empty;
    public string City { get; init; }
    public string Street { get; init; }
    public string Building { get; init; }
    public string? AddressDescription { get; init; }
    public string? Country { get; init; }
    public double Weight { get; init; }
    public double Height { get; init; }
    public string OwnersPhoneNumber { get; init; } = string.Empty;
    public bool IsNeutered { get; init; }
    public DateTime DateOfBirth { get; init; }
    public bool IsVaccinated { get; init; }
    public string HelpStatus { get; init; }
    public int Position { get; set; }

    public PetFileDto[] PetFiles { get; set; } = null!;
}