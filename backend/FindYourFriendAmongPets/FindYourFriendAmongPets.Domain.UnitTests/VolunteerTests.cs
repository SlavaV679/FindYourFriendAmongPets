using FindYourFriendAmongPets.Core.Models;
using FindYourFriendAmongPets.Core.Models.SpeciesAggregate;
using FindYourFriendAmongPets.Core.Shared.ValueObject;
using FluentAssertions;

namespace FindYourFriendAmongPets.Domain.UnitTests;

public class VolunteerTests
{
    [Fact]
    public void Add_Pet_With_Empty_Pets_Return_Success_Result()
    {
        // arrange
        var volunteer = CreateVolunteer();

        var petId = PetId.NewPetId();

        var pet = CreatePet(petId);

        // act
        var result = volunteer.AddPet(pet);
        var addedPetResult = volunteer.GetPetById(petId);

        // assert
        result.IsSuccess.Should().BeTrue();
        addedPetResult.IsSuccess.Should().BeTrue();
        addedPetResult.Value.Id.Should().Be(pet.Id);
        addedPetResult.Value.Position.Should().Be(Position.First);
    }

    [Fact]
    public void Add_Pet_With_Other_Pets_Return_Success_Result()
    {
        // arrange
        const int petCount = 5;

        var volunteer = CreateVolunteer();
        //AddPetsToVolunteer(volunteer, petCount);

        var pets = Enumerable.Range(1, petCount).Select(_ => CreatePet(PetId.NewPetId()));

        foreach (var petItem in pets)
            volunteer.AddPet(petItem);
        
        var petId = PetId.NewPetId();

        var petToAdd = CreatePet(petId);

        // act
        var result = volunteer.AddPet(petToAdd);
        var addedPetResult = volunteer.GetPetById(petId);
        
        var position = Position.Create(petCount + 1).Value;

        // assert
        result.IsSuccess.Should().BeTrue();
        addedPetResult.IsSuccess.Should().BeTrue();
        addedPetResult.Value.Id.Should().Be(petToAdd.Id);
        addedPetResult.Value.Position.Should().Be(position);
    }

    [Fact]
    public void MovePet_Should_Not_Move_When_Pet_Already_At_New_Position()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var secondPosition = Position.Create(2).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // act
        var result = volunteer.MovePet(secondPet, secondPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Should().Be(Position.Create(1).Value);
        secondPet.Position.Should().Be(Position.Create(2).Value);
        thirdPet.Position.Should().Be(Position.Create(3).Value);
        fourthPet.Position.Should().Be(Position.Create(4).Value);
        fifthPet.Position.Should().Be(Position.Create(5).Value);
    }
    
    [Fact]
    public void MovePet_Should_Move_Other_Pets_Forward_When_New_Position_Is_Lower()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var secondPosition = Position.Create(2).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // act
        var result = volunteer.MovePet(fourthPet, secondPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Should().Be(Position.Create(1).Value);
        secondPet.Position.Should().Be(Position.Create(3).Value);
        thirdPet.Position.Should().Be(Position.Create(4).Value);
        fourthPet.Position.Should().Be(Position.Create(2).Value);
        fifthPet.Position.Should().Be(Position.Create(5).Value);
    }
    
    [Fact]
    public void MovePet_Should_Move_Other_Pets_Back_When_New_Position_Is_Grater()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var fourthPosition = Position.Create(4).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // act
        var result = volunteer.MovePet(secondPet, fourthPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Should().Be(Position.Create(1).Value);
        secondPet.Position.Should().Be(Position.Create(4).Value);
        thirdPet.Position.Should().Be(Position.Create(2).Value);
        fourthPet.Position.Should().Be(Position.Create(3).Value);
        fifthPet.Position.Should().Be(Position.Create(5).Value);
    }
    
    [Fact]
    public void MovePet_Should_Move_Other_Pets_Forward_When_New_Position_Is_First()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var firstPosition = Position.Create(1).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // act
        var result = volunteer.MovePet(fifthPet, firstPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Should().Be(Position.Create(2).Value);
        secondPet.Position.Should().Be(Position.Create(3).Value);
        thirdPet.Position.Should().Be(Position.Create(4).Value);
        fourthPet.Position.Should().Be(Position.Create(5).Value);
        fifthPet.Position.Should().Be(Position.Create(1).Value);
    }
    
    [Fact]
    public void MovePet_Should_Move_Other_Pets_Back_When_New_Position_Is_Last()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var fifthPosition = Position.Create(5).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // act
        var result = volunteer.MovePet(firstPet, fifthPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Should().Be(Position.Create(5).Value);
        secondPet.Position.Should().Be(Position.Create(1).Value);
        thirdPet.Position.Should().Be(Position.Create(2).Value);
        fourthPet.Position.Should().Be(Position.Create(3).Value);
        fifthPet.Position.Should().Be(Position.Create(4).Value);
    }
    
    [Fact]
    public void MovePet_Should_Move_Other_Pets_Back_When_New_Position_Is_Out_Of_Range()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var tenthPosition = Position.Create(10).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // act
        var result = volunteer.MovePet(secondPet, tenthPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Should().Be(Position.Create(1).Value);
        secondPet.Position.Should().Be(Position.Create(5).Value);
        thirdPet.Position.Should().Be(Position.Create(2).Value);
        fourthPet.Position.Should().Be(Position.Create(3).Value);
        fifthPet.Position.Should().Be(Position.Create(4).Value);
    }
    
    private Volunteer CreateVolunteer()
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

    private Pet CreatePet(PetId petId)
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