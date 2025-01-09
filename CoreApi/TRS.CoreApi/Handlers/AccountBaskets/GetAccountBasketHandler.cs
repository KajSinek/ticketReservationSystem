using Helpers.Responses;
using Helpers.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TRS.CoreApi.Entities;
using TRS.CoreApi.Handlers.Accounts;

namespace TRS.CoreApi.Handlers.AccountBaskets;

public class GetAccountBasketCommand : IRequest<EntityResponse<AccountBasket>>
{
    public required Guid AccountId { get; set; }
}

public class GetAccountBasketHandler(IBaseDbRequests baseDbRequests, IMediator mediator, ILogger<AccountBasket> logger)
    : IRequestHandler<GetAccountBasketCommand, EntityResponse<AccountBasket>>
{
    public async Task<EntityResponse<AccountBasket>> Handle(
        GetAccountBasketCommand request,
        CancellationToken ct
    )
    {
        var accountResponse = await mediator.Send(new GetAccountHandlerQuery { AccountId = request.AccountId }, ct);
        if (accountResponse.Entity is null)
        {
            logger.LogError("Error with getting Account.");
            return new EntityResponse<AccountBasket>
            {
                Errors = accountResponse.Errors,
                StatusCode = accountResponse.StatusCode
            };
        }

        var accountBasketResponse = await baseDbRequests.GetAsync<AccountBasket>(request.AccountId, query => query.Include(x => x.AccountBasketTickets));
        if (accountBasketResponse.Entity is null)
        {
            logger.LogError("Failed to get Account Basket");
            return accountBasketResponse;
        }
        return accountBasketResponse;
    }
}