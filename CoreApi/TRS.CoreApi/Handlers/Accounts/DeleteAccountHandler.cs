using Helpers.Services;
using MediatR;
using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Handlers.Accounts;

public class DeleteAccountHandlerCommand : IRequest<Response>
{
    public required Guid AccountId { get; set; }
}

public class DeleteAccountHandler(IBaseDbRequests baseDbRequests)
    : IRequestHandler<DeleteAccountHandlerCommand, Response>
{
    public async Task<Response> Handle(DeleteAccountHandlerCommand request, CancellationToken ct)
    {
        var response = await baseDbRequests.DeleteAsync<Account>(request.AccountId);

        return response;
    }
}
