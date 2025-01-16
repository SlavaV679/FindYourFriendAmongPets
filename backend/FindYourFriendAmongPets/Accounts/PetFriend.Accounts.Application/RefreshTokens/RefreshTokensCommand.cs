using PetFriend.Core.Abstractions;

namespace PetFriend.Accounts.Application.RefreshTokens;

public record RefreshTokensCommand(Guid RefreshToken) : ICommand;