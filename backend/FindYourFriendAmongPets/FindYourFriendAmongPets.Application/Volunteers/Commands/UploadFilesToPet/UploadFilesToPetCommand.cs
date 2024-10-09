using FindYourFriendAmongPets.Application.Abstraction;
using FindYourFriendAmongPets.Application.Dtos;

namespace FindYourFriendAmongPets.Application.Volunteers.Commands.UploadFilesToPet;

public record UploadFilesToPetCommand(Guid VolunteerId, Guid PetId, IEnumerable<UploadFileDto> Files): ICommand;