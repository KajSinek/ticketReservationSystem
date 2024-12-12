using Helpers.Responses;
using Helpers.Services;
using MediatR;
using TRS.CoreApi.Entities;
using TRS.CoreApi.Interfaces;

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
    ILogger<CreateTicketHandler> logger,
    IBackgroundJobCall backgroundJobCall
) : IRequestHandler<CreateTicketHandlerCommand, EntityResponse<Ticket>>
{
    public async Task<EntityResponse<Ticket>> Handle(
        CreateTicketHandlerCommand request,
        CancellationToken ct
    )
    {
        var entity = new Ticket
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price,
            ExpirationDate = request.ExpirationDate,
            ValidityStartDate = request.ValidityStartDate
        };

        var ticketResponse = await baseDbRequests.CreateAsync(entity);

        if (ticketResponse.Entity is null)
        {
            logger.LogError("Failed to create Ticket");
            return ticketResponse;
        }

        try
        {
            // Log request before calling
            logger.LogInformation("Calling background job API...");
            var test = await backgroundJobCall.GetDataAsync(); // This is your Refit call

            // Log after calling
            logger.LogInformation("Background job API call completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Failed to call background job API: {ex.Message}");
        }

        return ticketResponse;
    }
}
