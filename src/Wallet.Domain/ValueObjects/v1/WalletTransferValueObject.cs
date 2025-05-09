using Wallet.Domain.Entities.v1;

namespace Wallet.Domain.ValueObjects.v1;

public record WalletTransferValueObject(UserWallet FromWallet, WalletTransaction FromWalletTransaction, UserWallet ToWallet, WalletTransaction ToWalletTransaction)
{
    public UserWallet FromWallet { get; } = FromWallet;
    public WalletTransaction FromWalletTransaction { get; } = FromWalletTransaction;
    public UserWallet ToWallet { get; set; } = ToWallet;
    public WalletTransaction ToWalletTransaction { get; } = ToWalletTransaction;
}