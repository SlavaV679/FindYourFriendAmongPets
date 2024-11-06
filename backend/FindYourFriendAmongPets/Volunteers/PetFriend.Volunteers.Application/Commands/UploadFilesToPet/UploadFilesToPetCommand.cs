using PetFriend.Core.Abstractions;
using PetFriend.Core.Dtos;

namespace PetFriend.Volunteers.Application.Commands.UploadFilesToPet;

public record UploadFilesToPetCommand(Guid VolunteerId, Guid PetId, IEnumerable<UploadFileDto> Files): ICommand;