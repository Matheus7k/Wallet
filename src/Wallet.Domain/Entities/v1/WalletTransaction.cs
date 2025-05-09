using Wallet.Domain.Enums.v1;

namespace Wallet.Domain.Entities.v1;

public class WalletTransaction
{
    public Guid Id { get; set; }
    public Guid WalletId { get; set; }
    public string From { get; set; } = null!;
    public string To { get; set; } = null!;
    public decimal Amount { get; set; }
    public string Transaction { get; set; } = null!;
    public StatusType Status { get; set; }
    public DateTime CreatedAt { get; set; }
}