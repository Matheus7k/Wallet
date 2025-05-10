using Wallet.Domain.Entities.v1;

namespace Wallet.Domain.Interfaces.v1.Repositories;

public interface IUserQueryRepository
{
    Task<IEnumerable<WalletTransaction>> GetTransactionsAsync(string emaiil, DateTime? startDate, DateTime? endDate);
}