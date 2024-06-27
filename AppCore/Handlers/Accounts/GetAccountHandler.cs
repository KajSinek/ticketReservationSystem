using AppCore.Entities;
using Helpers.Responses;
using Helpers.Services;
using MediatR;

namespace AppCore.Handlers.Accounts;

public class GetAccountHandlerQuery : IRequest<EntityResponse<Account>>
{
    public required Guid AccountId { get; set; }
}

public class GetAccountHandler(IBaseDbRequests baseDbRequests) : IRequestHandler<GetAccountHandlerQuery, EntityResponse<Account>>
{
    public async Task<EntityResponse<Account>> Handle(GetAccountHandlerQuery request, CancellationToken cancellationToken)
    {
        var data = await baseDbRequests.GetAsync<Account>(request.AccountId);
        return data;
    }
}
