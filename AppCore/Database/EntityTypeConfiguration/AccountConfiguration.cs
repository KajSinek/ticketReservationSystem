using AppCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
using AppCore.Resources;
namespace AppCore.Database.EntityTypeConfiguration;

[ExcludeFromCodeCoverage]
public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");
        builder.HasKey(x => x.AccountId);

        builder.HasIndex(x => x.Username)
            .IsUnique()
            .HasAnnotation("Error_Message_Username", ErrorMessages.AccountUsernameConflict);

        builder.Property(x => x.Username)
            .IsRequired();

        builder.Property(x => x.Email)
            .IsRequired()
            .HasAnnotation("Error_Message_Email", ErrorMessages.AccountEmailConflict);

        builder.HasIndex(x => x.Email)
            .IsUnique();
    }
}
