namespace TRS.CoreApi.Entities;

public class AccountBasketTicket
{
    public required Guid AccountBasketId { get; set; }
    public virtual AccountBasket? AccountBasket { get; set; }
    public required Guid TicketId { get; set; }
    public virtual Ticket? Ticket { get; set; }
    public required int Amount { get; set; }
    public required DateTime CreatedOn { get; set; }
    public DateTime LastTimeEdited { get; set; }
}
