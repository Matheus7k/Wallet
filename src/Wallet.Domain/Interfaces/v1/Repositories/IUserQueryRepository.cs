using Wallet.Domain.Entities.v1;

namespace Wallet.Domain.Interfaces.v1.Repositories;

public interface IUserQueryRepository
{
    Task<(IEnumerable<WalletTransaction>, IEnumerable<WalletTransaction>)> GetTransactionsAsync(string email, DateTime? startDate, DateTime? endDate);
}