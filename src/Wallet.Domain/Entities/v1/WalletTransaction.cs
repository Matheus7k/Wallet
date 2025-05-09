using Wallet.Domain.Enums.v1;

namespace Wallet.Domain.Entities.v1;

public class WalletTransaction
{
    public Guid Id { get; set; }
    public Guid WalletId { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public decimal Amount { get; set; }
    public string Transaction { get; set; }
    public StatusType Status { get; set; }
    public DateTime CreatedAt { get; set; }
}