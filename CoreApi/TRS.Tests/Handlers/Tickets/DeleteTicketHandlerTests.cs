using Helpers.Responses;
using Helpers.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TRS.CoreApi.Entities;
using TRS.CoreApi.Handlers.Tickets;

namespace TRS.Tests.Handlers.Tickets;

public class DeleteTicketHandlerTests : TestBase
{
    private readonly IBaseDbRequests _baseDbRequests;
    private readonly DeleteTicketHandler _handler;

    public DeleteTicketHandlerTests()
    {
        _baseDbRequests = Substitute.For<IBaseDbRequests>();
        _handler = Substitute.For<DeleteTicketHandler>(_baseDbRequests);
    }

    [Fact]
    public async Task Handle_WhenCalled_ReturnsTicket()
    {
        // Arrange
        var ticketId = Guid.NewGuid();
        var ticket = GetTestTicket(ticketId);

        var command = new DeleteTicketHandlerCommand { TicketId = ticket.TicketId };

        var response = new Response { Errors = new List<string>() };

        _baseDbRequests.DeleteAsync<Ticket>(Arg.Any<Guid>()).Returns(response);

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

        var command = new DeleteTicketHandlerCommand { TicketId = ticket.TicketId };

        var response = new Response { Errors = new List<string> { "Error" } };

        _baseDbRequests.DeleteAsync<Ticket>(Arg.Any<Guid>()).Returns(response);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Errors);
    }
}
