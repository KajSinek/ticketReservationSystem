using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TRS.CoreApi.Entities;
using TRS.CoreApi.Resources;

namespace TRS.CoreApi.Database.EntityTypeConfiguration;

[ExcludeFromCodeCoverage]
public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Username).IsRequired();
        builder
            .HasIndex(x => x.Username)
            .IsUnique()
            .HasAnnotation("Error_Message_Username", ErrorMessages.AccountUsernameConflict);

        builder.Property(x => x.Email).IsRequired();
        builder.HasIndex(x => x.Email).IsUnique().HasAnnotation("Error_Message_Email", ErrorMessages.AccountEmailConflict);

        builder.Property(x => x.FirstName).IsRequired();
        builder.Property(x => x.LastName).IsRequired();
        builder.Property(x => x.Address).IsRequired();
        builder.Property(x => x.PhoneNumber).IsRequired();

        builder.HasOne(x => x.AccountBalance)
            .WithOne(x => x.Account)
            .HasForeignKey<AccountBalance>(x => x.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.AccountBasket)
            .WithOne()
            .HasForeignKey<AccountBasket>(x => x.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
