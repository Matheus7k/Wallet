using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities.v1;

namespace Wallet.Infrastructure.Data.Command.Context.v1;

public class ContextDb(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<UserWallet> Wallets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("wallet");
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContextDb).Assembly);
    }
}