using TRS.CoreApi.Dtos;
using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Mapping;

public static class AccountBasketTicketMapping
{
    public static AccountBasketTicketApiDto ToAccountBasketTicketApiDto(this AccountBasketTicket accountBasketTicket) => new()
    {
        TicketId = accountBasketTicket.TicketId,
        BasketId = accountBasketTicket.AccountBasketId,
        Amount = accountBasketTicket.Amount,
    };
}
