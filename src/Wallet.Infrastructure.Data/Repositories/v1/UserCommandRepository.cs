using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities.v1;
using Wallet.Domain.Interfaces.v1.Repositories;
using Wallet.Domain.ValueObjects.v1;
using Wallet.Infrastructure.Data.Command.Context.v1;

namespace Wallet.Infrastructure.Data.Command.Repositories.v1;

public class UserCommandRepository(ContextDb context) : IUserCommandRepository
{
    public async Task<User?> GetUserByEmailAsync(string email) =>
        await context.Users.FirstOrDefaultAsync(x => x.Email == email);
    
    public async Task<Guid> GetUserIdByEmailAsync(string email) =>
        (await context.Users.FirstOrDefaultAsync(x => x.Email == email))!.Id;
    
    public async Task<UserWallet> GetWalletByIdAsync(Guid id) =>
        (await context.Wallets.FirstOrDefaultAsync(x => x.UserId == id))!;
    
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
            throw;
        }
    }
    
    public async Task UpdateUserWalletAsync(UserWallet wallet, WalletTransaction walletTransaction)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            context.Wallets.Update(wallet);
            
            context.WalletTransactions.Add(walletTransaction);
            
            await context.SaveChangesAsync();
            
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task UpdateTransferWalletsAsync(WalletTransferValueObject walletTransfer)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            context.Wallets.Update(walletTransfer.FromWallet);
            
            context.WalletTransactions.Add(walletTransfer.FromWalletTransaction);
            
            await context.SaveChangesAsync();
            
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}