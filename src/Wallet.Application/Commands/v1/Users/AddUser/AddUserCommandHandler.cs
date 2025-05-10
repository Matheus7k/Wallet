using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Wallet.CrossCutting.Exception;
using Wallet.Domain.Entities.v1;
using Wallet.Domain.Interfaces.v1.Repositories;

namespace Wallet.Application.Commands.v1.Users.AddUser;

public class AddUserCommandHandler(
    IUserCommandRepository userCommandRepository,
    IMapper mapper,
    ILogger<AddUserCommandHandler> logger) : IRequestHandler<AddUserCommand>
{ 
    public async Task Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await CheckUserEmailAlreadyExistsAsync(request.Email);

            await AddUserAsync(request);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Handler} => Erro ao cadastrar usuário. Request: {Request}", nameof(AddUserCommandHandler), request);
            throw;
        }
    }

    private async Task CheckUserEmailAlreadyExistsAsync(string email)
    {
        var user = await userCommandRepository.GetUserByEmailAsync(email);

        if (user is not null)
            throw new ConflictException("Email_AlreadyExist");
    }

    private async Task AddUserAsync(AddUserCommand request)
    {
        var user = mapper.Map<User>(request);

        var wallet = new UserWallet
        {
            UserId = user.Id,
            Balance = 0.00m,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        
        await userCommandRepository.AddUserAsync(user, wallet);
    }
}