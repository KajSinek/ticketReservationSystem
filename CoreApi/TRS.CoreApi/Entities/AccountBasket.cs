namespace TRS.CoreApi.Entities;

public class AccountBasket
{
    public required Guid Id { get; set; }
    public required Guid AccountId { get; set; }
    public required DateTime CreatedOn { get; set; }
    public DateTime LastTimeEdited { get; set; }
    public virtual ICollection<AccountBasketTicket>? AccountBasketTickets { get; set; }
}
