using Helpers.Responses;
using Helpers.Services;
using MediatR;
using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Handlers.AccountBalances;

public class CreateAccountBalanceCommand : IRequest<EntityResponse<AccountBalance>>
{
    public required Guid AccountId { get; set; }
}

public class CreateAccountBalanceHandler(
    IBaseDbRequests baseDbRequests,
    ILogger<UpdateAccountBalanceHandler> logger
) : IRequestHandler<CreateAccountBalanceCommand, EntityResponse<AccountBalance>>
{
    public async Task<EntityResponse<AccountBalance>> Handle(
        CreateAccountBalanceCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new AccountBalance
        {
            Id = Guid.NewGuid(),
            AccountId = request.AccountId,
            Value = 0
        };
        var accountBalanceResponse = await baseDbRequests.CreateAsync(entity);
        if (accountBalanceResponse.Entity is null)
        {
            logger.LogError("Failed to create Account Balance");
            return accountBalanceResponse;
        }

        return accountBalanceResponse;
    }
}
