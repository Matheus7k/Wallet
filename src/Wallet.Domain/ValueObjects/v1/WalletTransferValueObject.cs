using Wallet.Domain.Entities.v1;

namespace Wallet.Domain.ValueObjects.v1;

public record WalletTransferValueObject(UserWallet FromWallet, WalletTransaction FromWalletTransaction)
{
    public UserWallet FromWallet { get; } = FromWallet;
    public WalletTransaction FromWalletTransaction { get; } = FromWalletTransaction;
}