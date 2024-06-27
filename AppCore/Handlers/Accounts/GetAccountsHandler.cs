using AppCore.Entities;
using Helpers.Responses;
using Helpers.Services;
using MediatR;

namespace AppCore.Handlers.Accounts;

public class GetAccountsHandlerQuery : IRequest<EntitiesResponse<Account>>
{
}

public class GetAccountsHandler(IBaseDbRequests baseDbRequests) : IRequestHandler<GetAccountsHandlerQuery, EntitiesResponse<Account>>
{
    public async Task<EntitiesResponse<Account>> Handle(GetAccountsHandlerQuery request, CancellationToken cancellationToken)
    {
        var list = await baseDbRequests.GetAllAsync<Account>();
        return list;
    }
}
