using Helpers.Responses;
using Helpers.Services;
using MediatR;
using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Handlers.Tickets;

public class GetTicketHandlerQuery : IRequest<EntityResponse<Ticket>>
{
    public required Guid TicketId { get; set; }
}

public class GetTicketHandler(IBaseDbRequests baseDbRequest, ILogger<GetTicketHandler> logger)
    : IRequestHandler<GetTicketHandlerQuery, EntityResponse<Ticket>>
{
    public async Task<EntityResponse<Ticket>> Handle(
        GetTicketHandlerQuery request,
        CancellationToken cancellationToken
    )
    {
        var response = await baseDbRequest.GetAsync<Ticket>(request.TicketId);

        if (response.Entity is null)
        {
            logger.LogError("Failed to get Ticket");
            return response;
        }

        return response;
    }
}
