using MediatR;

namespace Wallet.Application.Commands.v1.Authenticate.PostAuthenticate;

public sealed record PostAuthenticateCommand : IRequest<PostAuthenticateCommandResponse>
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}