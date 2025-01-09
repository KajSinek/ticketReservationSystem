using Helpers.Resources;
using Helpers.Responses;
using Helpers.Services;
using Helpers.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TRS.CoreApi.Entities;
using TRS.CoreApi.Handlers.Accounts;
using TRS.CoreApi.Handlers.Tickets;

namespace TRS.CoreApi.Handlers.AccountBasketItems;

public class  UpdateAccountBasketItemCommand : IRequest<EntityResponse<AccountBasketTicket>>
{
    public required Guid AccountId { get; set; }
    public required Guid TicketId { get; set; }
    public required int Amount { get; set; }
}

public class UpdateAccountBasketItemHandler(IBaseDbRequests baseRequest,
                                            IMediator mediator,
                                             ILogger<UpdateAccountBasketItemHandler> logger)
    : IRequestHandler<UpdateAccountBasketItemCommand, EntityResponse<AccountBasketTicket>>
{
    public async Task<EntityResponse<AccountBasketTicket>> Handle(
        UpdateAccountBasketItemCommand request,
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

        var accountBasketTicketResponse = await baseRequest.GetAsync<AccountBasketTicket>(new object[] { accountBasket.Id, ticket.Id });
        if (accountBasketTicketResponse.Entity is null || accountBasketTicketResponse.Entity is not AccountBasketTicket accountBasketTicket)
        {
            logger.LogError("Failed to get Account Basket Item");
            return accountBasketTicketResponse;
        }

        var entity = new AccountBasketTicket
        {
            AccountBasketId = accountBasket.Id,
            TicketId = request.TicketId,
            Amount = request.Amount,
            CreatedOn = accountBasketTicket.CreatedOn,
            LastTimeEdited = DateHelper.DateTimeUtcNow
        };

        var updatedAccountBasketTicketResponse = await baseRequest.UpdateAsync(entity, accountBasket.Id, ticket.Id);
        if (updatedAccountBasketTicketResponse.Entity is null || updatedAccountBasketTicketResponse.Entity is not AccountBasketTicket updatedAccountBasketTicket)
        {
            logger.LogError("Failed to create Account Basket Item");
            return updatedAccountBasketTicketResponse;
        }
        return updatedAccountBasketTicketResponse;
    }
}
