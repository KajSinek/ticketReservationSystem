using AppCore.Entities;

namespace AppCore.Dtos;

public class TicketApiDto(Ticket ticket)
{
    public Guid TicketId { get; set; } = ticket.TicketId;
    public string Name { get; set; } = ticket.Name;
    public decimal Price { get; set; } = ticket.Price;
    public DateOnly ExpirationDate { get; set; } = ticket.ExpirationDate;
    public DateOnly ValidityStartDate { get; set; } = ticket.ValidityStartDate;
}
