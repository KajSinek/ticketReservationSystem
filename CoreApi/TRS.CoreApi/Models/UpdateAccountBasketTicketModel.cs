namespace TRS.CoreApi.Models;

public class UpdateAccountBasketTicketModel
{
    public required Guid TicketId { get; set; }
    public required int Amount { get; set; }
}
