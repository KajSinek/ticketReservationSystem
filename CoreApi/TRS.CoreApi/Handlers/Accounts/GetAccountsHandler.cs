using Helpers.Responses;
using Helpers.Services;
using MediatR;
using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Handlers.Accounts;

public class GetAccountsHandlerQuery : IRequest<EntitiesResponse<Account>> { }

public class GetAccountsHandler(IBaseDbRequests baseDbRequests)
    : IRequestHandler<GetAccountsHandlerQuery, EntitiesResponse<Account>>
{
    public async Task<EntitiesResponse<Account>> Handle(
        GetAccountsHandlerQuery request,
        CancellationToken cancellationToken
    )
    {
        var response = await baseDbRequests.GetAllAsync<Account>();
        return response;
    }
}
