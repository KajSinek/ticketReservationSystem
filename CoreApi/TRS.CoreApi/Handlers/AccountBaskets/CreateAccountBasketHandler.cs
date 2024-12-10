using Helpers.Resources;
using Helpers.Responses;
using Helpers.Services;
using MediatR;
using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Handlers.AccountBaskets;

public class CreateAccountBasketCommand : IRequest<EntityResponse<AccountBasket>>
{
    public required Guid AccountId { get; set; }
}

public class CreateAccountBasketHandler(IBaseDbRequests baseDbRequests, ILogger<AccountBasket> logger)
    : IRequestHandler<CreateAccountBasketCommand, EntityResponse<AccountBasket>>
{
    public async Task<EntityResponse<AccountBasket>> Handle(
        CreateAccountBasketCommand request,
        CancellationToken cancellationToken
    )
    {
        var accountResponse = await baseDbRequests.GetAsync<Account>(request.AccountId);
        if (accountResponse.Entity is null || accountResponse.Entity is not Account account)
        {
            logger.LogError("Account does not exist");
            return new EntityResponse<AccountBasket>
            {
                Errors = [string.Format(ErrorMessages.EntityNotFound, nameof(Account), request.AccountId)]
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
