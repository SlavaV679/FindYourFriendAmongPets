using CSharpFunctionalExtensions;
using FindYourFriendAmongPets.Core.Shared;

namespace FindYourFriendAmongPets.Core.Models;

public record PetRequisiteDetails
{
    public IReadOnlyList<Requisite> Requisites { get; }

    //ef core
    private PetRequisiteDetails()
    {
    }

    public PetRequisiteDetails(List<Requisite> requisites)
    {
        Requisites = requisites;
    }
}