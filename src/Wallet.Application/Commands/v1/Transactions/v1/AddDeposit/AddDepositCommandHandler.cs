using MediatR;
using Microsoft.Extensions.Logging;
using Wallet.CrossCutting.Exception;
using Wallet.Domain.Entities.v1;
using Wallet.Domain.Enums.v1;
using Wallet.Domain.Interfaces.v1.Factories;
using Wallet.Domain.Interfaces.v1.Repositories;

namespace Wallet.Application.Commands.v1.Transactions.v1.AddDeposit;

public class AddDepositCommandHandler(
    IUserCommandRepository userCommandRepository,
    IWalletTransactionFactory walletTransactionFactory,
    ILogger<AddDepositCommandHandler> logger) : IRequestHandler<AddDepositCommand>
{
    public async Task Handle(AddDepositCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var wallet = await GetUserWalletAsync(request.Email);

            ValidateUserWallet(wallet);

            var walletTransaction = walletTransactionFactory.CreateWalletTransaction(TransactionType.Deposit, new(wallet.Id, request.Email, request.Amount));

            await UpdateUserWalletAsync(wallet, request.Amount, walletTransaction);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Handler} => Erro ao realizar depósito. Request: {Request}", nameof(AddDepositCommandHandler), request);
            throw;
        }
    }

    private async Task<UserWallet> GetUserWalletAsync(string email)
    {
        var userId = await userCommandRepository.GetUserIdByEmailAsync(email);
            
        return await userCommandRepository.GetWalletByIdAsync(userId);
    }
    
    private static void ValidateUserWallet(UserWallet userWallet)
    {
        if (!userWallet.IsActive)
            throw new BadRequestException("WalletDeposit_Inactive");
    }

    private async Task UpdateUserWalletAsync(UserWallet userWallet, decimal amount, WalletTransaction walletTransaction)
    {
        userWallet.Balance += amount;
        userWallet.UpdatedAt = DateTime.UtcNow;
        
        await userCommandRepository.UpdateUserWalletAsync(userWallet, walletTransaction);
    }
}