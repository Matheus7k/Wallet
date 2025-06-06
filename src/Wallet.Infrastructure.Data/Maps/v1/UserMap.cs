using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallet.Domain.Entities.v1;

namespace Wallet.Infrastructure.Data.Command.Maps.v1;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.Property(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Password).IsRequired().HasMaxLength(255);
        builder.Property(x => x.BirthDate).IsRequired();
        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();

        builder.HasIndex(x => x.Email).IsUnique();

        builder.HasOne(c => c.Address)
            .WithOne()
            .HasForeignKey<User>(n => n.AddressId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(c => c.UserWallet)
            .WithOne()
            .HasForeignKey<UserWallet>(n => n.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}