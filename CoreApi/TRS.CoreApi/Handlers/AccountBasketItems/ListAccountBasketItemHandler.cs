using Helpers.Resources;
using Helpers.Responses;
using Helpers.Services;
using MediatR;
using TRS.CoreApi.Entities;
using TRS.CoreApi.Handlers.Accounts;

namespace TRS.CoreApi.Handlers.AccountBasketItems;

public class ListAccountBasketItemCommand : IRequest<EntitiesResponse<AccountBasketTicket>>
{
    public required Guid AccountId { get; set; }
}

public class ListAccountBasketItemHandler(IBaseDbRequests baseRequest,
                                         IMediator mediator,
                                         ILogger<ListAccountBasketItemHandler> logger)
    : IRequestHandler<ListAccountBasketItemCommand, EntitiesResponse<AccountBasketTicket>>
{
    public async Task<EntitiesResponse<AccountBasketTicket>> Handle(
        ListAccountBasketItemCommand request,
        CancellationToken ct
    )
    {
        var accountResponse = await mediator.Send(new GetAccountHandlerQuery { AccountId = request.AccountId }, ct);
        if (accountResponse.Entity is null || accountResponse.Entity is not Account account || account.AccountBasket is not AccountBasket accountBasket)
        {
            logger.LogError("Account does not exist");
            return new EntitiesResponse<AccountBasketTicket>
            {
                Errors = new List<string> { string.Format(ErrorMessages.EntityNotFound, nameof(Account), request.AccountId) }
            };
        }

        var accountBasketTicketResponse = await baseRequest.GetAllAsync<AccountBasketTicket>(x => x.Where(x => x.AccountBasketId == accountBasket.Id));
        if (accountBasketTicketResponse.Entities is null || accountBasketTicketResponse.Entities is not List<AccountBasketTicket> accountBasketTickets)
        {
            logger.LogError("Failed to create Account Basket Item");
            return accountBasketTicketResponse;
        }
        return accountBasketTicketResponse;
    }
}
