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
        builder.Property(x => x.From).IsRequired();
        builder.Property(x => x.To).IsRequired();
        builder.Property(x => x.Amount).IsRequired();
        builder.Property(x => x.Transaction).IsRequired();
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.CreatedAt);
    }
}