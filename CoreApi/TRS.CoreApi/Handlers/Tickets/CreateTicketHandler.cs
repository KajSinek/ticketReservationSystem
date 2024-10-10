using Helpers.Responses;
using Helpers.Services;
using MediatR;
using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Handlers.Tickets;

public class CreateTicketHandlerCommand : IRequest<EntityResponse<Ticket>>
{
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public DateOnly ExpirationDate { get; set; }
    public DateOnly ValidityStartDate { get; set; }
}

public class CreateTicketHandler(
    IBaseDbRequests baseDbRequests,
    ILogger<CreateTicketHandler> logger
) : IRequestHandler<CreateTicketHandlerCommand, EntityResponse<Ticket>>
{
    public async Task<EntityResponse<Ticket>> Handle(
        CreateTicketHandlerCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new Ticket
        {
            TicketId = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price,
            ExpirationDate = request.ExpirationDate,
            ValidityStartDate = request.ValidityStartDate
        };

        var response = await baseDbRequests.CreateAsync(entity);

        if (response.Entity is null)
        {
            logger.LogError("Failed to create Ticket");
            return response;
        }

        return response;
    }
}
