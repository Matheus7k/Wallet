using Wallet.Domain.Entities.v1;
using Wallet.Domain.ValueObjects.v1;

namespace Wallet.Domain.Interfaces.v1.Repositories;

public interface IUserCommandRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    Task AddUserAsync(User user, UserWallet wallet);
    Task<UserWallet> GetUserWalletByEmailAsync(string email);
    Task UpdateUserWalletAsync(UserWallet wallet, WalletTransaction walletTransaction);
    Task UpdateTransferWalletsAsync(WalletTransferValueObject walletTransfer);
}