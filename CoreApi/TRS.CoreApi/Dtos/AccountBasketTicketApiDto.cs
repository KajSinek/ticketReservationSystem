using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Dtos;

public class AccountBasketTicketApiDto
{
    public Guid TicketId { get; set; }
    public Guid BasketId { get; set; }
    public int Amount { get; set; }
}
