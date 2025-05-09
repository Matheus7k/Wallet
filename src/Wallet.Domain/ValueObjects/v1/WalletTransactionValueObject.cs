using Wallet.Domain.Enums.v1;

namespace Wallet.Domain.ValueObjects.v1;

public sealed record WalletTransactionValueObject(Guid WalletId, string FromEmail, string ToEmail, decimal Amount)
{
    public Guid WalletId { get; } = WalletId;
    public string FromEmail { get; } = FromEmail;
    public string ToEmail { get; } = ToEmail;
    public decimal Amount { get; } = Amount;
}