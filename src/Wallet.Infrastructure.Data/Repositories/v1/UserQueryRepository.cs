using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities.v1;
using Wallet.Domain.Enums.v1;
using Wallet.Domain.Interfaces.v1.Repositories;
using Wallet.Domain.ValueObjects.v1;
using Wallet.Infrastructure.Data.Command.Context.v1;

namespace Wallet.Infrastructure.Data.Command.Repositories.v1;

public class UserQueryRepository(ContextDb context) : IUserQueryRepository
{
    public async Task<(IEnumerable<WalletTransaction>, IEnumerable<WalletTransaction>)> GetPaginatedTransactionsAsync(PaginatedTransactionsValueObject paginatedTransactionsVo)
    {
        var wallet = await GetUserWalletByEmailAsync(paginatedTransactionsVo.Email);
        
        var sentTransactionsQuery = context.WalletTransactions
            .Where(w => w.FromWalletId == wallet.Id);
        
        var receivedTransactionsQuery = context.WalletTransactions
            .Where(w => w.ToWalletId == wallet.Id && w.Transaction != nameof(TransactionType.Deposit));

        if (paginatedTransactionsVo.StartDate.HasValue)
        {
            var start = paginatedTransactionsVo.StartDate.Value.ToUniversalTime();
            var end = paginatedTransactionsVo.EndDate!.Value.ToUniversalTime();
        
            sentTransactionsQuery = sentTransactionsQuery.Where(u => u.CreatedAt.Date >= start && u.CreatedAt.Date <= end);
            receivedTransactionsQuery = receivedTransactionsQuery.Where(u => u.CreatedAt.Date >= start && u.CreatedAt.Date <= end);
        }
        
        var skip = paginatedTransactionsVo.Page * paginatedTransactionsVo.PageSize;
        
        return (await sentTransactionsQuery.Skip(skip).Take(paginatedTransactionsVo.PageSize).ToListAsync(),
            await receivedTransactionsQuery.Skip(skip).Take(paginatedTransactionsVo.PageSize).ToListAsync());
    }
    
    public async Task<UserWallet> GetUserWalletByEmailAsync(string email) =>
        (await context.Users.Include(u => u.UserWallet)
            .Where(u => u.Email == email)
            .FirstAsync()).UserWallet;

    public async Task<int> GetTotalRowsAsync(string email)
    {
        var wallet = await GetUserWalletByEmailAsync(email);

        var sentCount = await context.WalletTransactions
            .Where(w => w.FromWalletId == wallet.Id)
            .CountAsync();
        
        var receivedCount = await context.WalletTransactions
            .Where(w => w.ToWalletId == wallet.Id && w.Transaction != nameof(TransactionType.Deposit))
            .CountAsync();
        
        return Math.Max(sentCount, receivedCount);
    }
}