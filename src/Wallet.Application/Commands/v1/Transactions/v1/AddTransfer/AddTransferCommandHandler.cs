using MediatR;
using Microsoft.Extensions.Logging;
using Wallet.CrossCutting.Exception;
using Wallet.Domain.Entities.v1;
using Wallet.Domain.Enums.v1;
using Wallet.Domain.Interfaces.v1.Factories;
using Wallet.Domain.Interfaces.v1.Repositories;
using Wallet.Domain.ValueObjects.v1;

namespace Wallet.Application.Commands.v1.Transactions.v1.AddTransfer;

public class AddTransferCommandHandler(
    IUserCommandRepository userCommandRepository,
    IWalletTransactionFactory walletTransactionFactory,
    ILogger<AddTransferCommandHandler> logger) : IRequestHandler<AddTransferCommand>
{
    public async Task Handle(AddTransferCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await ValidateToUserAsync(request.ToEmail);

            var (fromWallet, toWallet) = await GetUserWalletsAsync(request);

            ValidateWallets(fromWallet, toWallet, request.Amount);

            await RealizeTransferAsync(fromWallet, toWallet, request);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Handler} => Erro ao realizar transferencia entre contas. Request: {Request}", nameof(AddTransferCommandHandler), request);
            throw;
        }
    }

    private async Task ValidateToUserAsync(string toEmail)
    {
        var user = await userCommandRepository.GetUserByEmailAsync(toEmail);
        
        if (user is null)
            throw new NotFoundException("UserTransfer_NotFound");
    }
    
    private async Task<(UserWallet fromWallet, UserWallet toWallet)> GetUserWalletsAsync(AddTransferCommand request)
    {
        var fromWalletId = await userCommandRepository.GetUserIdByEmailAsync(request.FromEmail);
        var toWalletId = await userCommandRepository.GetUserIdByEmailAsync(request.ToEmail);
        
        var fromWallet = await userCommandRepository.GetWalletByIdAsync(fromWalletId);
        var toWallet = await userCommandRepository.GetWalletByIdAsync(toWalletId);
        
        return (fromWallet, toWallet);
    }
    
    private static void ValidateWallets(UserWallet fromWallet, UserWallet toWallet, decimal amount)
    {
        if (!fromWallet.IsActive || !toWallet.IsActive)
            throw new BadRequestException("WalletTransfer_Inactive");

        if (fromWallet.Balance < amount)
            throw new ConflictException("WalletTransfer_Balance");
    }

    private async Task RealizeTransferAsync(UserWallet fromWallet, UserWallet toWallet, AddTransferCommand request)
    {
        fromWallet.Balance -= request.Amount;
        toWallet.Balance += request.Amount;
        
        var now = DateTime.UtcNow;

        fromWallet.UpdatedAt = now;
        toWallet.UpdatedAt = now;

        var walletTransactionVo = CreateWalletTransactionValueObject(fromWallet.Id, request.FromEmail, toWallet.Id, request.ToEmail, request.Amount);
        
        var walletTransaction = CreateWalletTransaction(walletTransactionVo);
        
        await userCommandRepository.UpdateTransferWalletsAsync(new(fromWallet, walletTransaction));
    }

    private static WalletTransactionValueObject CreateWalletTransactionValueObject(Guid fromWalletId, string fromEmail, Guid toWalletId, string toEmail, decimal amount) =>
        new(fromWalletId, fromEmail, amount, toWalletId, toEmail);
    
    private WalletTransaction CreateWalletTransaction(WalletTransactionValueObject walletTransactionValueObject) =>
        walletTransactionFactory.CreateWalletTransaction(TransactionType.Transfer, walletTransactionValueObject);
}