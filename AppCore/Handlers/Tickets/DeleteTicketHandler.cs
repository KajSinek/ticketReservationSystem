﻿using AppCore.Entities;
using Helpers.Services;
using MediatR;

namespace AppCore.Handlers.Tickets;

public class DeleteTicketHandlerCommand : IRequest<Response>
{
    public required Guid TicketId { get; set; }
}

public class DeleteTicketHandler(IBaseDbRequests baseDbRequests) : IRequestHandler<DeleteTicketHandlerCommand, Response>
{
    public async Task<Response> Handle(DeleteTicketHandlerCommand request, CancellationToken cancellationToken)
    {
        var response = await baseDbRequests.DeleteAsync<Ticket>(request.TicketId);

        return response;
    }
}