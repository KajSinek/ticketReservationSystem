using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Database.EntityTypeConfiguration;

[ExcludeFromCodeCoverage]
public class AccountBasketConfiguration : IEntityTypeConfiguration<AccountBasket>
{
    public void Configure(EntityTypeBuilder<AccountBasket> builder)
    {
        builder.ToTable("AccountBaskets");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AccountId).IsRequired();
        builder.Property(x => x.CreatedOn).IsRequired();

        builder.HasMany(x => x.AccountBasketTickets)
            .WithOne(x => x.AccountBasket)
            .HasForeignKey(x => x.AccountBasketId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
