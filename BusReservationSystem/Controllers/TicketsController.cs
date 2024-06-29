using AppCore.Entities;
using AppCore.Handlers.Tickets;
using AppCore.Models;
using Helpers.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers;

[ApiController]
public class TicketsController(IMediator mediator) : ControllerBase
{
    public const string GetTicketRouteName = "GetTicket";

    [HttpPost]
    [Route("api/tickets")]
    [SwaggerOperation("AddTicket")]
    [SwaggerResponse(201, "Ticket added successfully")]
    [SwaggerResponse(400, "Invalid input")]
    [SwaggerResponse(409, "Ticket Conflict")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EntityResponse<Ticket>))]
    public async Task<IActionResult> CreateTicketAsync([FromBody, Bind] CreateTicketModel model, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateTicketHandlerCommand
        {
            Name = model.Name,
            Price = model.Price,
            ExpirationDate = model.ExpirationDate,
            ValidityStartDate = model.ValidityStartDate
        }
        , ct);

        if (result.IsFailure || result.Entity is null) return StatusCode((int)result.StatusCode, result);

        return CreatedAtRoute(GetTicketRouteName, routeValues: new { ticket_id = result.Entity.TicketId }, value: result);
    }

    [HttpGet]
    [Route("api/tickets")]
    [SwaggerOperation("GetTickets")]
    [SwaggerResponse(200, "Tickets getted successfully")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EntitiesResponse<Ticket>))]
    public async Task<IActionResult> GetTicketsAsync(CancellationToken ct)
    {
        var result = await mediator.Send(new GetTicketsHandlerQuery { }, ct);
        return Ok(result);
    }

    [HttpGet]
    [Route("api/tickets/{ticket_id}", Name = GetTicketRouteName)]
    [SwaggerOperation("GetTicket")]
    [SwaggerResponse(200, "Ticket getted successfully")]
    [SwaggerResponse(400, "Invalid input")]
    [SwaggerResponse(404, "Ticket not found")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EntityResponse<Ticket>))]
    public async Task<IActionResult> GetTicketAsync([Required, FromRoute(Name = "ticket_id")] Guid ticket_id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetTicketHandlerQuery { TicketId = ticket_id }, ct);

        if (result.IsFailure || result.Entity is null) return StatusCode((int)result.StatusCode, result);

        return Ok(result);
    }

    [HttpPut]
    [Route("api/tickets/{ticket_id}")]
    [SwaggerOperation("UpdateTicket")]
    [SwaggerResponse(200, "Ticket updated successfully")]
    [SwaggerResponse(400, "Invalid input")]
    [SwaggerResponse(404, "Ticket not found")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EntityResponse<Ticket>))]
    public async Task<IActionResult> UpdateTicketAsync([Required, FromRoute(Name = "ticket_id")] Guid ticket_id, [FromBody, Bind] UpdateTicketModel model, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateTicketHandlerCommand
        {
            TicketId = ticket_id,
            Name = model.Name,
            Price = model.Price,
            ExpirationDate = model.ExpirationDate,
            ValidityStartDate = model.ValidityStartDate
        }
        , ct);

        if (result.IsFailure || result.Entity is null) return StatusCode((int)result.StatusCode, result);

        return Ok(result);
    }

    [HttpDelete]
    [Route("api/tickets/{ticket_id}")]
    [SwaggerOperation("DeleteTicket")]
    [SwaggerResponse(204, "Ticket deleted successfully")]
    [SwaggerResponse(400, "Invalid input")]
    [SwaggerResponse(404, "Ticket not found")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteTicketAsync([Required, FromRoute(Name = "ticket_id")] Guid ticket_id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteTicketHandlerCommand { TicketId = ticket_id }, ct);

        if (result.IsFailure) return StatusCode((int)result.StatusCode, result);

        return NoContent();
    }
}
