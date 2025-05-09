using MediatR;
using Microsoft.Extensions.Logging;
using Wallet.Application.Commands.v1.Transactions.v1.AddTransfer;
using Wallet.CrossCutting.Exception;
using Wallet.Domain.Interfaces.v1.Repositories;
using Wallet.Domain.Interfaces.v1.Services;

namespace Wallet.Application.Commands.v1.Authenticate.PostAuthenticate;

public sealed class PostAuthenticateCommandHandler(
    IUserCommandRepository userCommandRepository,
    IPasswordEncryptorService passwordEncryptorService,
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
        
        if (user.Password != passwordEncryptorService.Encrypt(request.Password))
            throw new BadRequestException("Password_Invalid");
    }
}