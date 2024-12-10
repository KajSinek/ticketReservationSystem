using Helpers.Responses;
using Helpers.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Handlers.Accounts;

public class GetAccountHandlerQuery : IRequest<EntityResponse<Account>>
{
    public required Guid AccountId { get; set; }
}

public class GetAccountHandler(IBaseDbRequests baseDbRequests, ILogger<GetAccountHandler> logger)
    : IRequestHandler<GetAccountHandlerQuery, EntityResponse<Account>>
{
    public async Task<EntityResponse<Account>> Handle(
        GetAccountHandlerQuery request,
        CancellationToken cancellationToken
    )
    {
        var data = await baseDbRequests.GetAsync<Account>(request.AccountId, query => query.Include(x => x.AccountBalance));

        if (data.Entity is null)
        {
            logger.LogError("Failed to get Account");
            return data;
        }

        return data;
    }
}
