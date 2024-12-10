using Helpers.Responses;
using Helpers.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
        var response = await baseDbRequests.GetAllAsync<Account>(query => query.Include(x => x.AccountBalance));
        return response;
    }
}
