using Wallet.Domain.Enums.v1;

namespace Wallet.Domain.Entities.v1;

public class WalletTransaction
{
    public Guid Id { get; set; }
    public Guid FromWalletId { get; set; }
    public string FromEmail { get; set; } = null!;
    public Guid ToWalletId { get; set; }
    public string ToEmail { get; set; } = null!;
    public decimal Amount { get; set; }
    public string Transaction { get; set; } = null!;
    public StatusType Status { get; set; }
    public DateTime CreatedAt { get; set; }
}