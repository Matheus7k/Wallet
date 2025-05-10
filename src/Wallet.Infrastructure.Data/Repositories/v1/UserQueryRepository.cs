using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities.v1;
using Wallet.Domain.Interfaces.v1.Repositories;
using Wallet.Infrastructure.Data.Command.Context.v1;

namespace Wallet.Infrastructure.Data.Command.Repositories.v1;

public class UserQueryRepository(ContextDb context) : IUserQueryRepository
{
    public async Task<IEnumerable<WalletTransaction>> GetTransactionsAsync(string emaiil, DateTime? startDate, DateTime? endDate)
    {
        var wallet = await GetUserWalletByEmailAsync(emaiil);
        
        var query = context.WalletTransactions.Where(w => w.WalletId == wallet.Id);

        if (!startDate.HasValue) 
            return await query.ToListAsync();
        
        query = query.Where(u => u.CreatedAt.Date >= startDate.Value && u.CreatedAt.Date <= endDate!.Value);

        return await query.ToListAsync();
    }

    private async Task<UserWallet> GetUserWalletByEmailAsync(string email) =>
        (await context.Users.Include(u => u.UserWallet)
            .Where(u => u.Email == email)
            .FirstAsync()).UserWallet;
}