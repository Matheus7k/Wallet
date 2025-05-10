using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities.v1;
using Wallet.Domain.Enums.v1;
using Wallet.Domain.Interfaces.v1.Repositories;
using Wallet.Infrastructure.Data.Command.Context.v1;

namespace Wallet.Infrastructure.Data.Command.Repositories.v1;

public class UserQueryRepository(ContextDb context) : IUserQueryRepository
{
    public async Task<(IEnumerable<WalletTransaction>, IEnumerable<WalletTransaction>)> GetTransactionsAsync(string email, DateTime? startDate, DateTime? endDate)
    {
        var wallet = await GetUserWalletByEmailAsync(email);
        
        var sentTransactionsQuery = context.WalletTransactions.Where(w => w.FromWalletId == wallet.Id);
        
        var receivedTransactionsQuery = context.WalletTransactions.Where(w => w.ToWalletId == wallet.Id && w.Transaction != nameof(TransactionType.Deposit));

        if (!startDate.HasValue)
            return (await sentTransactionsQuery.ToListAsync(), await receivedTransactionsQuery.ToListAsync());
        
        sentTransactionsQuery = sentTransactionsQuery.Where(u => u.CreatedAt.Date >= startDate.Value.ToUniversalTime() && u.CreatedAt.Date <= endDate!.Value.ToUniversalTime());

        receivedTransactionsQuery = sentTransactionsQuery.Where(u => u.CreatedAt.Date >= startDate.Value.ToUniversalTime() && u.CreatedAt.Date <= endDate!.Value.ToUniversalTime());
        
        return (await sentTransactionsQuery.ToListAsync(), await receivedTransactionsQuery.ToListAsync());
    }

    private async Task<UserWallet> GetUserWalletByEmailAsync(string email) =>
        (await context.Users.Include(u => u.UserWallet)
            .Where(u => u.Email == email)
            .FirstAsync()).UserWallet;
}