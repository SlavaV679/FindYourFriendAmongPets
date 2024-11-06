namespace PetFriend.Volunteers.Domain.ValueObject;

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