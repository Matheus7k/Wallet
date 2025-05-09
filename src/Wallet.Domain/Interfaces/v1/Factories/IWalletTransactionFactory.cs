using Wallet.Domain.Entities.v1;
using Wallet.Domain.Enums.v1;
using Wallet.Domain.ValueObjects.v1;

namespace Wallet.Domain.Interfaces.v1.Factories;

public interface IWalletTransactionFactory
{
    WalletTransaction CreateWalletTransaction(TransactionType transaction, WalletTransactionValueObject walletTransaction);
}