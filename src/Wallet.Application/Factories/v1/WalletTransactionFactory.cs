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
            TransactionType.Deposit => GenerateTransaction(TransactionType.Deposit, StatusType.Pending, walletTransaction),
            _ => GenerateTransaction(TransactionType.Transfer, StatusType.Completed, walletTransaction)
        };
    
    private static WalletTransaction GenerateTransaction(TransactionType transactionType, StatusType status, WalletTransactionValueObject walletTransaction) =>
        new()
        {
            FromWalletId = walletTransaction.FromWalletId,
            FromEmail = walletTransaction.FromEmail,
            ToWalletId = walletTransaction.ToWalletId ?? walletTransaction.FromWalletId,
            ToEmail = walletTransaction.ToEmail ?? walletTransaction.FromEmail,
            Amount = walletTransaction.Amount,
            Transaction = transactionType.ToString(),
            Status = status,
            CreatedAt = DateTime.UtcNow
        };
}