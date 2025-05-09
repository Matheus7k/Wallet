using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities.v1;
using Wallet.Domain.Interfaces.v1.Repositories;
using Wallet.Infrastructure.Data.Command.Context.v1;

namespace Wallet.Infrastructure.Data.Command.Repositories.v1;

public class UserCommandRepository(ContextDb context) : IUserCommandRepository
{
    public async Task<User?> GetUserByEmailAsync(string email) =>
        await context.Users.FirstOrDefaultAsync(x => x.Email == email);
}