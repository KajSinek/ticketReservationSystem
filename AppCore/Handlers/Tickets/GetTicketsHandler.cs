using AppCore.Entities;
using Helpers.Responses;
using Helpers.Services;
using MediatR;

namespace AppCore.Handlers.Tickets;

public class GetTicketsHandlerQuery : IRequest<EntitiesResponse<Ticket>> { }

public class GetTicketsHandler(IBaseDbRequests baseDbRequests) : IRequestHandler<GetTicketsHandlerQuery, EntitiesResponse<Ticket>>
{
    public async Task<EntitiesResponse<Ticket>> Handle(GetTicketsHandlerQuery request, CancellationToken ct)
    {
        var response = await baseDbRequests.GetAllAsync<Ticket>();

        return response;
    }
}
