namespace AppCore.Entities;

public class Ticket
{
    public required Guid TicketId { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public DateOnly ExpirationDate { get; set; }
    public DateOnly ValidityStartDate { get; set; }
}
