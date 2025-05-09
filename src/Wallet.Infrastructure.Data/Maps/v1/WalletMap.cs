using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallet.Domain.Entities.v1;

namespace Wallet.Infrastructure.Data.Command.Maps.v1;

public class WalletMap : IEntityTypeConfiguration<UserWallet>
{
    public void Configure(EntityTypeBuilder<UserWallet> builder)
    {
        builder.ToTable("Wallets");
        
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Balance).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
    }
}