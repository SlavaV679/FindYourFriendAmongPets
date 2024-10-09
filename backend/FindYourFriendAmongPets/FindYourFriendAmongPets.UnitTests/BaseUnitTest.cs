using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Models.SpeciesAggregate;
using FindYourFriendAmongPets.Core.Shared.ValueObject;

namespace FindYourFriendAmongPets.UnitTests;


public class BaseUnitTest
{
    [Fact]
    public async Task SemaphoreSlimTest()
    {
        var semaphoreSlim = new SemaphoreSlim(3);

        var tasks = Enumerable.Range(0, 10).Select(async _ =>
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                await AsyncMethod();
            }
            finally
            {
                semaphoreSlim.Release();
            }
        });

        await Task.WhenAll(tasks);
    }

    private async Task AsyncMethod()
    {
        Console.WriteLine("Before uploading");
        await Task.Delay(2000);
        Console.WriteLine("After uploading");
    }
    
    public static Volunteer CreateVolunteer()
    {
        var socialNetwork = new SocialNetwork(Guid.NewGuid(), "Title", "Link");
        var requisiteForHelp = new RequisiteForHelp(Guid.NewGuid(), "Name", "Description");

        var volunteer = Volunteer.Create(
            VolunteerId.NewVolunteerId(),
            FullName.Create("FirstName", "LastName", "Patronymic").Value,
            Description.Create("Description").Value,
            PhoneNumber.Create("+20240917001").Value,
            [requisiteForHelp],
            [socialNetwork],
            1,
            1,
            1,
            1);

        return volunteer.Value;
    }

    public static Pet CreatePet(PetId petId)
    {
        var requisite = Requisite.Create($"Requisite name", $"description");
        var requisiteDetails = new PetRequisiteDetails([requisite.Value]);

        PetFile petFile = new PetFile(FilePath.Create("fullPath.jpg").Value, false);
        var petPhotos = new ValueObjectList<PetFile>([petFile]);

        var pet = Pet.Create(
            petId,
            $"Pet",
            PetSpecies.Create(SpeciesId.NewSpeciesId(), Guid.NewGuid()).Value,
            Description.Create("Description").Value,
            "orange",
            "health info",
            Address.Create("City", "Street", "building", "description").Value,
            10,
            20,
            PhoneNumber.Create("+20240918101").Value,
            true,
            DateTime.Now,
            true,
            Status.LookingForHome,
            requisiteDetails,
            petPhotos
        );

        return pet.Value;
    }

    private Volunteer CreateVolunteerWithPets(int petCount)
    {
        var volunteer = CreateVolunteer();
        
        for (int i = 0; i < petCount; i++)
        {
            var requisite = Requisite.Create($"Requisite name-{i}", $"description-{i}");
            var requisiteDetails = new PetRequisiteDetails([requisite.Value]);

            PetFile petFile = new PetFile(FilePath.Create("fullPath.jpg").Value, false);
            var petPhotos = new ValueObjectList<PetFile>([petFile]);

            var pet = Pet.Create(
                PetId.NewPetId(),
                $"Pet-{i}",
                PetSpecies.Create(SpeciesId.NewSpeciesId(), Guid.NewGuid()).Value,
                Description.Create("Description").Value,
                "orange",
                "health info",
                Address.Create("City", "Street", "building", "description").Value,
                10,
                20,
                PhoneNumber.Create("+20240918101").Value,
                true,
                DateTime.Now,
                true,
                Status.LookingForHome,
                requisiteDetails,
                petPhotos
            );

            volunteer.AddPet(pet.Value);
        }

        return volunteer;
    }
}