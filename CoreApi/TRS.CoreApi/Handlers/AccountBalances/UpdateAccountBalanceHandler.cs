using Helpers.Responses;
using Helpers.Services;
using MediatR;
using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Handlers.AccountBalances;

public class UpdateAccountBalanceHandlerCommand : IRequest<EntityResponse<AccountBalance>>
{
    public required Guid AccountId { get; set; }
    public required decimal Value { get; set; }
}

public class UpdateAccountBalanceHandler(
    IBaseDbRequests baseDbRequests,
    IMediator mediator,
    ILogger<UpdateAccountBalanceHandler> logger
) : IRequestHandler<UpdateAccountBalanceHandlerCommand, EntityResponse<AccountBalance>>
{
    public async Task<EntityResponse<AccountBalance>> Handle(
        UpdateAccountBalanceHandlerCommand request,
        CancellationToken ct
    )
    {
        var accountBalanceResponse = await mediator.Send(new GetAccountBalanceQuery
        {
            AccountId = request.AccountId
        },
        ct);

        if (accountBalanceResponse.Entity is null
            || accountBalanceResponse.Entity is not AccountBalance accountBalance)
        {
            logger.LogError("Error with getting Account Balance");
            return accountBalanceResponse;
        }

        var entity = new AccountBalance
        {
            Id = accountBalance.Id,
            AccountId = request.AccountId,
            Value = accountBalance.Value + request.Value
        };

        var response = await baseDbRequests.UpdateAsync(accountBalance.Id, entity);

        if (response.Entity is null)
        {
            logger.LogError("Failed to update Account Balance");
            return response;
        }

        return response;
    }
}
