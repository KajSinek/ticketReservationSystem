namespace TRS.CoreApi.Models;

public class CreateAccountBasketItemModel
{
    public required Guid TicketId { get; set; }
    public required int Amount { get; set; }
}
