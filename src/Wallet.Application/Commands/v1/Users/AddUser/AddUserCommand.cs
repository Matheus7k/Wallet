using MediatR;
using Wallet.Domain.Entities.v1;

namespace Wallet.Application.Commands.v1.Users.AddUser;

public sealed record AddUserCommand : IRequest
{
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; set; } = null!;
    public DateTime BirthDate { get; init; }
    public Address Address { get; init; } = null!;
}