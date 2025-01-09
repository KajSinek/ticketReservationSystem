using Helpers.Resources;
using Helpers.Responses;
using Helpers.Services;
using MediatR;
using TRS.CoreApi.Entities;
using TRS.CoreApi.Handlers.Accounts;
using TRS.CoreApi.Handlers.Tickets;

namespace TRS.CoreApi.Handlers.AccountBasketItems;

public class DeleteAccountBasketItemCommand : IRequest<Response>
{
    public required Guid AccountId { get; set; }
    public required Guid TicketId { get; set; }
}

public class DeleteAccountBasketItemHandler(IBaseDbRequests baseRequest,
                                            IMediator mediator,
                                            ILogger<DeleteAccountBasketItemHandler> logger)
    : IRequestHandler<DeleteAccountBasketItemCommand, Response>
{
    public async Task<Response> Handle(
        DeleteAccountBasketItemCommand request,
        CancellationToken ct
    )
    {
        var accountResponse = await mediator.Send(new GetAccountHandlerQuery { AccountId = request.AccountId }, ct);
        if (accountResponse.Entity is null || accountResponse.Entity is not Account account || account.AccountBasket is not AccountBasket accountBasket)
        {
            logger.LogError("Account does not exist");
            return new EntityResponse<AccountBasketTicket>
            {
                Errors = new List<string> { string.Format(ErrorMessages.EntityNotFound, nameof(Account), request.AccountId) }
            };
        }

        var ticketResponse = await mediator.Send(new GetTicketHandlerQuery { TicketId = request.TicketId }, ct);
        if (ticketResponse.Entity is null || ticketResponse.Entity is not Ticket ticket)
        {
            logger.LogError("Ticket does not exist");
            return new EntityResponse<AccountBasketTicket>
            {
                Errors = new List<string> { string.Format(ErrorMessages.EntityNotFound, nameof(Ticket), request.TicketId) }
            };
        }

        var accountBasketTicketResponse = await baseRequest.DeleteAsync<AccountBasketTicket>(accountBasket.Id, ticket.Id);
        if (accountBasketTicketResponse.IsFailure)
        {
            logger.LogError("Failed to create Account Basket Item");
            return accountBasketTicketResponse;
        }
        return accountBasketTicketResponse;
    }
}
