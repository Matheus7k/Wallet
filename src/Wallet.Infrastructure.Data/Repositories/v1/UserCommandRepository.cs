using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities.v1;
using Wallet.Domain.Interfaces.v1.Repositories;
using Wallet.Infrastructure.Data.Command.Context.v1;

namespace Wallet.Infrastructure.Data.Command.Repositories.v1;

public class UserCommandRepository(ContextDb context) : IUserCommandRepository
{
    public async Task<User?> GetUserByEmailAsync(string email) =>
        await context.Users.FirstOrDefaultAsync(x => x.Email == email);
    
    public async Task AddUserAsync(User user, UserWallet wallet)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            await context.Users.AddAsync(user);

            await context.Addresses.AddAsync(user.Address);

            await context.SaveChangesAsync();
            
            await context.Wallets.AddAsync(wallet);
            
            await context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
        }
    }
}