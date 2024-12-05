using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Database.EntityTypeConfiguration;

[ExcludeFromCodeCoverage]
public class AccountBalanceConfiguration : IEntityTypeConfiguration<AccountBalance>
{
    public void Configure(EntityTypeBuilder<AccountBalance> builder)
    {
        builder.ToTable("AccountBalances");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AccountId).IsRequired();
        builder.Property(x => x.Value).IsRequired();

        builder.HasOne(x => x.Account)
            .WithOne(x => x.AccountBalance)
            .HasForeignKey<AccountBalance>(x => x.AccountId);
    }
}
