using Helpers.Models;
using Helpers.Responses;
using Helpers.Services;
using MediatR;
using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Handlers.AccountBalances;

public class GetAccountBalanceQuery : IRequest<EntityResponse<AccountBalance>>
{
    public required Guid AccountId { get; set; }
}

public class GetAccountBalanceHandler(
    IBaseDbRequests baseDbRequests,
    ILogger<GetAccountBalanceHandler> logger
    ) : IRequestHandler<GetAccountBalanceQuery, EntityResponse<AccountBalance>>
{
    public async Task<EntityResponse<AccountBalance>> Handle(
        GetAccountBalanceQuery request,
        CancellationToken cancellationToken
    )
    {
        var accountBalanceResponse = await baseDbRequests.GetEntityByPropertyAsync(
            new GetEntityByPropertyModel<AccountBalance>
            {
                Query = x => x.AccountId == request.AccountId
            }
        );
        if (
            accountBalanceResponse.Entity is null
            || accountBalanceResponse.Entity is not AccountBalance accountBalance
        )
        {
            logger.LogError("Error with getting Account Balance");
            return accountBalanceResponse;
        }
        return accountBalanceResponse;
    }
}
