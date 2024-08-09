using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TRS.CoreApi.Entities;
using TRS.CoreApi.Resources;

namespace TRS.CoreApi.Database.EntityTypeConfiguration;

[ExcludeFromCodeCoverage]
public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");
        builder.HasKey(x => x.TicketId);

        builder.Property(x => x.Name).IsRequired();

        builder
            .HasIndex(x => x.Name)
            .IsUnique()
            .HasAnnotation("Error_Message_Ticket", ErrorMessages.TicketNameConflict);

        builder.Property(x => x.Price).IsRequired();

        builder.Property(x => x.ExpirationDate).IsRequired();

        builder.Property(x => x.ValidityStartDate).IsRequired();
    }
}
