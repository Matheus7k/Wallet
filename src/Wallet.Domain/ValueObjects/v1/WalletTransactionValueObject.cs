using Wallet.Domain.Enums.v1;

namespace Wallet.Domain.ValueObjects.v1;

public sealed record WalletTransactionValueObject(Guid FromWalletId, string FromEmail, decimal Amount, Guid? ToWalletId = null, string ToEmail = null!)
{
    public Guid FromWalletId { get; } = FromWalletId;
    public string FromEmail { get; } = FromEmail;
    public Guid? ToWalletId { get; } = ToWalletId;
    public string? ToEmail { get; } = ToEmail;
    public decimal Amount { get; } = Amount;
}