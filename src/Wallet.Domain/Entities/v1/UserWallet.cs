namespace Wallet.Domain.Entities.v1;

public class UserWallet
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public decimal Balance { get; set; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}