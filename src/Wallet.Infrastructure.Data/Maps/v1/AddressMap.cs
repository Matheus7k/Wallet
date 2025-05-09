using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallet.Domain.Entities.v1;

namespace Wallet.Infrastructure.Data.Command.Maps.v1;

public class AddressMap : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Street).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Number).IsRequired();
        builder.Property(x => x.City).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Country).IsRequired().HasMaxLength(100);
    }
}