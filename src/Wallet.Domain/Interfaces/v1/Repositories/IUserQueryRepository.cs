using Wallet.Domain.Entities.v1;
using Wallet.Domain.ValueObjects.v1;

namespace Wallet.Domain.Interfaces.v1.Repositories;

public interface IUserQueryRepository
{
    Task<(IEnumerable<WalletTransaction>, IEnumerable<WalletTransaction>)> GetPaginatedTransactionsAsync(PaginatedTransactionsValueObject paginatedTransactionsVo);
    Task<UserWallet> GetUserWalletByEmailAsync(string email);
    Task<int> GetTotalRowsAsync(string email);
}