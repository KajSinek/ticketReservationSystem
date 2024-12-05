using System.Net;
using Helpers.Responses;
using Helpers.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TRS.CoreApi.Entities;
using TRS.CoreApi.Handlers.Tickets;
using TRS.CoreApi.Resources;

namespace TRS.Tests.Handlers.Tickets;

public class GetTicketHandlerTests : TestBase
{
    private readonly IBaseDbRequests _baseDbRequests;
    private readonly ILogger<GetTicketHandler> _logger;
    private readonly GetTicketHandler _handler;

    public GetTicketHandlerTests()
    {
        _baseDbRequests = Substitute.For<IBaseDbRequests>();
        _logger = Substitute.For<ILogger<GetTicketHandler>>();
        _handler = Substitute.For<GetTicketHandler>(_baseDbRequests, _logger);
    }

    [Fact]
    public async Task Handle_WhenCalled_ReturnsTicket()
    {
        // Arrange
        var ticketId = Guid.NewGuid();
        var ticket = GetTestTicket(ticketId);

        var command = new GetTicketHandlerQuery { TicketId = ticket.Id };

        var response = new EntityResponse<Ticket> { Entity = ticket };

        _baseDbRequests.GetAsync<Ticket>(Arg.Any<Guid>()).Returns(response);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Handle_WhenGetAsyncReturnsNull_ThrowsException()
    {
        // Arrange
        var query = new GetTicketHandlerQuery { TicketId = Guid.NewGuid() };

        var response = new EntityResponse<Ticket>
        {
            StatusCode = HttpStatusCode.NotFound,
            Errors = new List<string>
            {
                string.Format(ErrorMessages.EntityNotFound, typeof(Ticket), query.TicketId)
            }
        };

        _baseDbRequests.GetAsync<Ticket>(Arg.Any<Guid>()).Returns(response);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Errors);
    }
}
