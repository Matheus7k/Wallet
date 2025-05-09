using MediatR;
using Microsoft.Extensions.Logging;
using Wallet.CrossCutting.Exception;
using Wallet.Domain.Entities.v1;
using Wallet.Domain.Enums.v1;
using Wallet.Domain.Interfaces.v1.Factories;
using Wallet.Domain.Interfaces.v1.Repositories;

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
            throw new BadRequestException("Conta de destino inválida ou inexistente.");
    }
    
    private async Task<(UserWallet fromWallet, UserWallet toWallet)> GetUserWalletsAsync(AddTransferCommand request)
    {
        var fromWallet = await userCommandRepository.GetUserWalletByEmailAsync(request.FromEmail);
        var toWallet = await userCommandRepository.GetUserWalletByEmailAsync(request.ToEmail);
        
        return (fromWallet, toWallet);
    }
    
    private static void ValidateWallets(UserWallet fromWallet, UserWallet toWallet, decimal amount)
    {
        if (!fromWallet.IsActive)
            throw new BadRequestException("Sua carteira está desativada, não é possivel realizar transferencias.");
        
        if (!toWallet.IsActive)
            throw new BadRequestException("A carteira de destino está desativada, não é possivel realizar transferencias.");
        
        if (fromWallet.Balance < amount)
            throw new ConflictException("Não possui fundos suficientes para realizar a transferencia.");
    }

    private async Task RealizeTransferAsync(UserWallet fromWallet, UserWallet toWallet, AddTransferCommand request)
    {
        fromWallet.Balance -= request.Amount;
        toWallet.Balance += request.Amount;
        
        var now = DateTime.UtcNow;

        fromWallet.UpdatedAt = now;
        toWallet.UpdatedAt = now;
        
        var fromWalletTransaction = CreateWalletTransaction(fromWallet.Id, request.FromEmail, request.ToEmail, request.Amount);
        var toWalletTransaction = CreateWalletTransaction(toWallet.Id, request.FromEmail, request.ToEmail, request.Amount);
        
        await userCommandRepository.UpdateTransferWalletsAsync(new(fromWallet, fromWalletTransaction, toWallet, toWalletTransaction));
    }
    
    private WalletTransaction CreateWalletTransaction(Guid walletId, string fromEmail, string toEmail, decimal amount) =>
        walletTransactionFactory.CreateWalletTransaction(TransactionType.Transfer, new(walletId, fromEmail, toEmail, amount));
}