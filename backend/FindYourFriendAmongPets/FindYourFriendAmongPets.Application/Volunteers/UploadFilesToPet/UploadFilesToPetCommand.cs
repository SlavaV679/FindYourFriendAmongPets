using FindYourFriendAmongPets.Application.Dtos;

namespace FindYourFriendAmongPets.Application.Volunteers.UploadFilesToPet;

public record UploadFilesToPetCommand(Guid VolunteerId, Guid PetId, IEnumerable<UploadFileDto> Files);