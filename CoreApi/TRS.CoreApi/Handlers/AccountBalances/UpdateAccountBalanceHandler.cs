using Helpers.Responses;
using Helpers.Services;
using MediatR;
using TRS.CoreApi.Entities;
using TRS.CoreApi.Enums;
using TRS.CoreApi.Resources;

namespace TRS.CoreApi.Handlers.AccountBalances;

public class UpdateAccountBalanceCommand : IRequest<EntityResponse<AccountBalance>>
{
    public required Guid AccountId { get; set; }
    public required decimal Value { get; set; }
    public required AccountBalanceType Type { get; set; }
}

public class UpdateAccountBalanceHandler(
    IBaseDbRequests baseDbRequests,
    IMediator mediator,
    ILogger<UpdateAccountBalanceHandler> logger
) : IRequestHandler<UpdateAccountBalanceCommand, EntityResponse<AccountBalance>>
{
    public async Task<EntityResponse<AccountBalance>> Handle(
        UpdateAccountBalanceCommand request,
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
            Value = default
        };

        if (request.Value > accountBalance.Value && request.Type == AccountBalanceType.Sub)
        {
            logger.LogError(string.Format(ErrorMessages.eAccountHasNotEnoughMoney, request.AccountId));
            return new EntityResponse<AccountBalance>
            {
                Errors =
                [
                    string.Format(ErrorMessages.eAccountHasNotEnoughMoney, request.AccountId) 
                ] 
            };
        }

        switch (request.Type)
        {
            case AccountBalanceType.Add: 
                entity.Value = accountBalance.Value + request.Value;
                break;
            case AccountBalanceType.Sub:
                entity.Value = accountBalance.Value - request.Value;
                break;
            default:
                entity.Value = accountBalance.Value;
                break;
        }


        var response = await baseDbRequests.UpdateAsync(accountBalance.Id, entity);

        if (response.Entity is null)
        {
            logger.LogError("Failed to update Account Balance");
            return response;
        }

        return response;
    }
}
