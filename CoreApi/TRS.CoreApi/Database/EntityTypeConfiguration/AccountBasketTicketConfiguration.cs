using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Database.EntityTypeConfiguration;

[ExcludeFromCodeCoverage]
public class AccountBasketTicketConfiguration : IEntityTypeConfiguration<AccountBasketTicket>
{
    public void Configure(EntityTypeBuilder<AccountBasketTicket> builder)
    {
        builder.ToTable("AccountBasketTickets");
        builder.HasKey(x => new { x.AccountBasketId, x.TicketId });
        builder.Property(x => x.AccountBasketId).IsRequired();
        builder.Property(x => x.TicketId).IsRequired();
        builder.Property(x => x.Amount).IsRequired();
        builder.Property(x => x.CreatedOn).IsRequired();

        builder.HasOne(x => x.AccountBasket)
            .WithMany(x => x.AccountBasketTickets)
            .HasForeignKey(x => x.AccountBasketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Ticket)
            .WithMany(x => x.AccountBasketTickets)
            .HasForeignKey(x => x.TicketId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
