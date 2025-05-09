using Wallet.Domain.Entities.v1;

namespace Wallet.Domain.Interfaces.v1.Repositories;

public interface IUserCommandRepository
{
    Task<User?> GetUserByEmailAsync(string email);
}