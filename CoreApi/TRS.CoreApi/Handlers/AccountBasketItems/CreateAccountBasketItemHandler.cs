using Helpers.Resources;
using Helpers.Responses;
using Helpers.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Handlers.AccountBasketItems;

public class  CreateAccountBasketItemCommand : IRequest<EntityResponse<AccountBasketTicket>>
{
    public required Guid AccountId { get; set; }
    public required Guid TicketId { get; set; }
    public required int Amount { get; set; }
}

public class CreateAccountBasketItemHandler(IBaseDbRequests baseRequest,
                                             ILogger<CreateAccountBasketItemHandler> logger)
{
    public async Task<EntityResponse<AccountBasketTicket>> Handle(
        CreateAccountBasketItemCommand request,
        CancellationToken cancellationToken
    )
    {
        var accountResponse = await baseRequest.GetAsync<Account>(
            request.AccountId, 
            query => query.Include(ab => ab.AccountBalance)
                            .Include(ab => ab.AccountBasket)
            );
        if (accountResponse.Entity is null || accountResponse.Entity is not Account account || account.AccountBasket is not AccountBasket accountBasket)
        {
            logger.LogError("Account does not exist");
            return new EntityResponse<AccountBasketTicket>
            {
                Errors = new List<string> { string.Format(ErrorMessages.EntityNotFound, nameof(Account), request.AccountId) }
            };
        }

        var ticketResponse = await baseRequest.GetAsync<Ticket>(request.TicketId);
        if (ticketResponse.Entity is null || ticketResponse.Entity is not Ticket ticket)
        {
            logger.LogError("Ticket does not exist");
            return new EntityResponse<AccountBasketTicket>
            {
                Errors = new List<string> { string.Format(ErrorMessages.EntityNotFound, nameof(Ticket), request.TicketId) }
            };
        }

        var entity = new AccountBasketTicket
        {
            AccountBasketId = accountBasket.Id,
            TicketId = request.TicketId,
            Amount = request.Amount,
            Ticket = ticket,
            CreatedOn = DateTime.Now
        };

        var accountBasketTicketResponse = await baseRequest.CreateAsync(entity);
        if (accountBasketTicketResponse.Entity is null || accountBasketTicketResponse.Entity is not AccountBasketTicket accountBasketTicket)
        {
            logger.LogError("Failed to create Account Basket Item");
            return accountBasketTicketResponse;
        }
        return accountBasketTicketResponse;
    }
}
