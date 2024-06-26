﻿using AppCore.Entities;
using Helpers.Responses;
using Helpers.Services;
using MediatR;

namespace AppCore.Handlers.Tickets;

public class UpdateTicketHandlerCommand : IRequest<EntityResponse<Ticket>>
{
    public required Guid TicketId { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public DateOnly ExpirationDate { get; set; }
    public DateOnly ValidityStartDate { get; set; }
}

public class UpdateTicketHandler(IBaseDbRequests baseDbRequests, ILogger<UpdateTicketHandler> logger) : IRequestHandler<UpdateTicketHandlerCommand, EntityResponse<Ticket>>
{
    public async Task<EntityResponse<Ticket>> Handle(UpdateTicketHandlerCommand request, CancellationToken cancellationToken)
    {
        var entity = new Ticket
        {
            TicketId = request.TicketId,
            Name = request.Name,
            Price = request.Price,
            ExpirationDate = request.ExpirationDate,
            ValidityStartDate = request.ValidityStartDate
        };

        var response = await baseDbRequests.UpdateAsync(request.TicketId, entity);

        if (response.Entity is null)
        {
            logger.LogError("Failed to update Ticket");
            return response;
        }

        return response;
    }
}
