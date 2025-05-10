using MediatR;
using Microsoft.Extensions.Logging;
using Wallet.CrossCutting.Exception;
using Wallet.Domain.Entities.v1;
using Wallet.Domain.Enums.v1;
using Wallet.Domain.Interfaces.v1.Factories;
using Wallet.Domain.Interfaces.v1.Repositories;
using Wallet.Domain.ValueObjects.v1;

namespace Wallet.Application.Commands.v1.Transactions.AddTransfer;

public class AddTransferCommandHandler(
    IUserCommandRepository userCommandRepository,
    IWalletTransactionFactory walletTransactionFactory,
    ILogger<AddTransferCommandHandler> logger) : IRequestHandler<AddTransferCommand>
{
    public async Task Handle(AddTransferCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await EnsureDestinationUserExists(request.ToEmail);

            var (fromWallet, toWallet) = await GetTransferWallets(request);

            ValidateTransfer(fromWallet, toWallet, request.Amount);

            await ExecuteTransfer(fromWallet, toWallet, request);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Handler} => Erro ao realizar transferencia entre contas. Request: {Request}", nameof(AddTransferCommandHandler), request);
            throw;
        }
    }

    private async Task EnsureDestinationUserExists
        (string toEmail)
    {
        var user = await userCommandRepository.GetUserByEmailAsync(toEmail);
        
        if (user is null)
            throw new NotFoundException("UserTransfer_NotFound");
    }
    
    private async Task<(UserWallet fromWallet, UserWallet toWallet)> GetTransferWallets(AddTransferCommand request)
    {
        var fromId = await userCommandRepository.GetUserIdByEmailAsync(request.FromEmail);
        var toId = await userCommandRepository.GetUserIdByEmailAsync(request.ToEmail);
        
        var fromWallet = await userCommandRepository.GetWalletByIdAsync(fromId);
        var toWallet = await userCommandRepository.GetWalletByIdAsync(toId);
        
        return (fromWallet, toWallet);
    }
    
    private static void ValidateTransfer(UserWallet fromWallet, UserWallet toWallet, decimal amount)
    {
        if (!fromWallet.IsActive || !toWallet.IsActive)
            throw new BadRequestException("WalletTransfer_Inactive");

        if (fromWallet.Balance < amount)
            throw new ConflictException("WalletTransfer_Balance");
    }

    private async Task ExecuteTransfer(UserWallet fromWallet, UserWallet toWallet, AddTransferCommand request)
    {
        fromWallet.Balance -= request.Amount;
        toWallet.Balance += request.Amount;
        
        var now = DateTime.UtcNow;

        fromWallet.UpdatedAt = now;
        toWallet.UpdatedAt = now;

        var transactionVo = new WalletTransactionValueObject(fromWallet.Id, request.FromEmail, request.Amount, toWallet.Id, request.ToEmail);
        
        var transaction = walletTransactionFactory.CreateWalletTransaction(TransactionType.Transfer, transactionVo);
        
        await userCommandRepository.UpdateTransferWalletsAsync(new(fromWallet, transaction));
    }
}