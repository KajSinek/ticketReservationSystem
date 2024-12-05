using Helpers.Resources;
using Helpers.Responses;
using Helpers.Services;
using MediatR;
using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Handlers.AccountBasketItems;

public class  CreateAccountBasketItemCommand : IRequest<EntityResponse<AccountBasket>>
{
    public required Guid AccountId { get; set; }
    public required Guid TicketId { get; set; }
    public required int Amount { get; set; }
}

public class CreateAccountBasketItemHandler (IBaseDbRequests baseRequest,
                                             ILogger<CreateAccountBasketItemHandler> logger)
{
    public async Task<EntityResponse<AccountBasket>> Handle(
        CreateAccountBasketItemCommand request,
        CancellationToken cancellationToken
    )
    {
        /*var accountResponse = await baseRequest.GetAsync<Account>(request.AccountId);
        if (accountResponse.Entity is null || accountResponse.Entity is not Account account)
        {
            logger.LogError("Account does not exist");
            return new EntityResponse<AccountBasket>
            {
                Errors = new List<string> { string.Format(ErrorMessages.EntityNotFound, nameof(Account), request.AccountId) }
            };
        }

        var ticketResponse = await baseRequest.GetAsync<Ticket>(request.TicketId);
        if (ticketResponse.Entity is null || ticketResponse.Entity is not Ticket ticket)
        {
            logger.LogError("Ticket does not exist");
            return new EntityResponse<AccountBasket>
            {
                Errors = new List<string> { string.Format(ErrorMessages.EntityNotFound, nameof(Ticket), request.TicketId) }
            };
        }

        var accountBasketResponse = await baseRequest.GetAsync<AccountBasket>(new object[] { account.AccountId, ticket.TicketId });

        var entity = new AccountBasket
        {
            AccountId = request.AccountId,
            TicketId = request.TicketId,
            Amount = request.Amount,
            Account = account,
            Ticket = ticket,
            CreatedOn = DateTime.Now
        };
        var response = await baseRequest.CreateAsync(entity);
        if (response.Entity is null || response.Entity is not AccountBasket accountBasketItem)
        {
            logger.LogError("Failed to create Account Basket Item");
            return response;
        }
        return response;*/
        return new EntityResponse<AccountBasket>();
    }
}
