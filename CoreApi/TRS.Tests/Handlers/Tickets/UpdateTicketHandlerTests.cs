using Helpers.Responses;
using Helpers.Services;
using Helpers.Utilities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TRS.CoreApi.Entities;
using TRS.CoreApi.Handlers.Tickets;

namespace TRS.Tests.Handlers.Tickets;

public class UpdateTicketHandlerTests : TestBase
{
    private readonly IBaseDbRequests _baseDbRequests;
    private readonly ILogger<UpdateTicketHandler> _logger;
    private readonly UpdateTicketHandler _handler;

    public UpdateTicketHandlerTests()
    {
        _baseDbRequests = Substitute.For<IBaseDbRequests>();
        _logger = Substitute.For<ILogger<UpdateTicketHandler>>();
        _handler = Substitute.For<UpdateTicketHandler>(_baseDbRequests, _logger);
    }

    [Fact]
    public async Task Handle_WhenCalled_ReturnsTicket()
    {
        // Arrange
        var ticketId = Guid.NewGuid();
        var ticket = GetTestTicket(ticketId);
        var updatedTicket = GetTestTicket(ticketId, name: "New Name");

        var command = new UpdateTicketHandlerCommand
        {
            TicketId = ticket.Id,
            Name = updatedTicket.Name,
            Price = updatedTicket.Price,
            ExpirationDate = updatedTicket.ExpirationDate,
            ValidityStartDate = updatedTicket.ValidityStartDate
        };

        var response = new EntityResponse<Ticket> { Entity = updatedTicket };

        _baseDbRequests.UpdateAsync(Arg.Any<Ticket>(), Arg.Any<Guid>()).Returns(response);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Handle_WhenCalled_ReturnsNull()
    {
        // Arrange
        var ticketId = Guid.NewGuid();
        var ticket = GetTestTicket(ticketId);
        var updatedTicket = GetTestTicket(ticketId, name: "New Name");

        var command = new UpdateTicketHandlerCommand
        {
            TicketId = ticket.Id,
            Name = updatedTicket.Name,
            Price = updatedTicket.Price,
            ExpirationDate = updatedTicket.ExpirationDate,
            ValidityStartDate = updatedTicket.ValidityStartDate
        };

        var response = new EntityResponse<Ticket>
        {
            Entity = null,
            Errors = new List<string> { "Error" }
        };

        _baseDbRequests.UpdateAsync(Arg.Any<Ticket>(), Arg.Any<Guid>()).Returns(response);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }
}
