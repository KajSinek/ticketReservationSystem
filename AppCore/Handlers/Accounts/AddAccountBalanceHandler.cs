using AppCore.Entities;
using Helpers.Models;
using Helpers.Responses;
using Helpers.Services;
using MediatR;

namespace AppCore.Handlers.Accounts;

public class AccountBalanceHandlerCommand : IRequest<EntityResponse<AccountBalance>>
{
    public required Guid AccountId { get; set; }
    public required decimal Value { get; set; }
}

public class AddAccountBalanceHandler(IBaseDbRequests baseDbRequests, ILogger<AddAccountBalanceHandler> logger) : IRequestHandler<AccountBalanceHandlerCommand,EntityResponse<AccountBalance>>
{
    public async Task<EntityResponse<AccountBalance>> Handle(AccountBalanceHandlerCommand request, CancellationToken cancellationToken)
    {
        var accountBalanceResponse = await baseDbRequests.GetEntityByPropertyAsync(new GetEntityByPropertyModel<AccountBalance>
        {
            Query = x => x.AccountId == request.AccountId
        });

        if (accountBalanceResponse.Entity is null || accountBalanceResponse.Entity is not AccountBalance accountBalance)
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
