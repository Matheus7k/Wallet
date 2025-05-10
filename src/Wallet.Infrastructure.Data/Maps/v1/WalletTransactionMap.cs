using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallet.Domain.Entities.v1;

namespace Wallet.Infrastructure.Data.Command.Maps.v1;

public class WalletTransactionMap : IEntityTypeConfiguration<WalletTransaction>
{
    public void Configure(EntityTypeBuilder<WalletTransaction> builder)
    {
        builder.ToTable("WalletTransactions");
        
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.FromWalletId).IsRequired();
        builder.Property(x => x.FromEmail).IsRequired();
        builder.Property(x => x.ToWalletId).IsRequired();
        builder.Property(x => x.ToEmail).IsRequired();
        builder.Property(x => x.Amount).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(x => x.Transaction).IsRequired();
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        
        builder.HasIndex(x => x.Id);
        builder.HasIndex(x => x.CreatedAt);
    }
}