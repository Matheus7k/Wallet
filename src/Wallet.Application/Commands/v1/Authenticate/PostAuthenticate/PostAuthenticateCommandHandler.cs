using MediatR;
using Microsoft.Extensions.Logging;
using Wallet.Application.Commands.v1.Transactions.AddTransfer;
using Wallet.CrossCutting.Exception;
using Wallet.Domain.Interfaces.v1.Repositories;
using Wallet.Domain.Interfaces.v1.Services;
using static BCrypt.Net.BCrypt;

namespace Wallet.Application.Commands.v1.Authenticate.PostAuthenticate;

public sealed class PostAuthenticateCommandHandler(
    IUserCommandRepository userCommandRepository,
    ITokenService tokenService,
    ILogger<PostAuthenticateCommandHandler> logger) 
    : IRequestHandler<PostAuthenticateCommand, PostAuthenticateCommandResponse>
{
    
    public async Task<PostAuthenticateCommandResponse> Handle(PostAuthenticateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await ValidationsAsync(request);

            var token = tokenService.GenerateToken(request.Email);

            return new PostAuthenticateCommandResponse(token);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Handler} => Erro ao realizar autenticação. Request: {Request}", nameof(AddTransferCommandHandler), request);
            throw;
        }
    }

    private async Task ValidationsAsync(PostAuthenticateCommand request)
    {
        var user = await userCommandRepository.GetUserByEmailAsync(request.Email);

        if (user is null)
            throw new NotFoundException("User_NotFound");
        
        if (!Verify(request.Password, user.Password))
            throw new BadRequestException("Password_Invalid");
    }
}