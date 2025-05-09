using Wallet.Domain.Entities.v1;
using Wallet.Domain.Enums.v1;
using Wallet.Domain.Interfaces.v1.Factories;
using Wallet.Domain.ValueObjects.v1;

namespace Wallet.Application.Factories.v1;

public class WalletTransactionFactory : IWalletTransactionFactory
{
    public WalletTransaction CreateWalletTransaction(TransactionType transaction, WalletTransactionValueObject walletTransaction) =>
        transaction switch
        {
            TransactionType.Deposit => GenerateDepositTransaction(walletTransaction),
            _ => GenerateTransferTransaction(walletTransaction)
        };

    private static WalletTransaction GenerateDepositTransaction(WalletTransactionValueObject walletTransaction) =>
        new()
        {
            WalletId = walletTransaction.WalletId,
            From = walletTransaction.FromEmail,
            To = walletTransaction.FromEmail,
            Amount = walletTransaction.Amount,
            Transaction = nameof(TransactionType.Deposit),
            Status = StatusType.Pending,
            CreatedAt = DateTime.UtcNow
        };
    
    private static WalletTransaction GenerateTransferTransaction(WalletTransactionValueObject walletTransaction) =>
        new()
        {
            WalletId = walletTransaction.WalletId,
            From = walletTransaction.FromEmail,
            To = walletTransaction.ToEmail,
            Amount = walletTransaction.Amount,
            Transaction = nameof(TransactionType.Transfer),
            Status = StatusType.Completed,
            CreatedAt = DateTime.UtcNow
        };
}