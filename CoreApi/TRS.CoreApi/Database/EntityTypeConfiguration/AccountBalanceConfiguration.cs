using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Database.EntityTypeConfiguration;

public class AccountBalanceConfiguration : IEntityTypeConfiguration<AccountBalance>
{
    public void Configure(EntityTypeBuilder<AccountBalance> builder)
    {
        builder.ToTable("AccountBalances");
        builder.HasKey(x => x.Id);
    }
}
