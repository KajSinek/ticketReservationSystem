using Helpers.Resources;
using Helpers.Responses;
using Helpers.Services;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using TRS.CoreApi.Entities;
using TRS.CoreApi.Handlers.Accounts;

namespace TRS.CoreApi.Handlers.AccountBaskets;

public class CreateAccountBasketCommand : IRequest<EntityResponse<AccountBasket>>
{
    public required Guid AccountId { get; set; }
}

public class CreateAccountBasketHandler(IBaseDbRequests baseDbRequests, IMediator mediator, ILogger<AccountBasket> logger)
    : IRequestHandler<CreateAccountBasketCommand, EntityResponse<AccountBasket>>
{
    public async Task<EntityResponse<AccountBasket>> Handle(
        CreateAccountBasketCommand request,
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

        var entity = new AccountBasket
        {
            Id = Guid.NewGuid(),
            AccountId = request.AccountId,
            CreatedOn = DateTime.Now
        };

        var response = await baseDbRequests.CreateAsync(entity);
        if (response.Entity is null || response.Entity is not AccountBasket accountBasket)
        {
            logger.LogError("Failed to create Account Basket");
            return response;
        }
        return response;
    }
}
