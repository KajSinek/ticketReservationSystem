using AppCore.Entities;
using AppCore.Handlers.Tickets;
using Helpers.Responses;
using Helpers.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace TRS.Tests.Handlers.Tickets;

public class CreateTicketHandlerTests : TestBase
{
    private readonly IBaseDbRequests _baseDbRequests;
    private readonly ILogger<CreateTicketHandler> _logger;
    private readonly CreateTicketHandler _handler;

    public CreateTicketHandlerTests()
    {
        _baseDbRequests = Substitute.For<IBaseDbRequests>();
        _logger = Substitute.For<ILogger<CreateTicketHandler>>();
        _handler = Substitute.For<CreateTicketHandler>(_baseDbRequests, _logger);
    }

    [Fact]
    public async Task Handle_WhenCalled_ReturnsTicket()
    {
        // Arrange
        var ticketId = Guid.NewGuid();
        var ticket = GetTestTicket(ticketId);

        var command = new CreateTicketHandlerCommand
        {
            Name = ticket.Name,
            Price = ticket.Price,
            ExpirationDate = ticket.ExpirationDate,
            ValidityStartDate = ticket.ValidityStartDate
        };

        var response = new EntityResponse<Ticket>
        {
            Entity = ticket
        };

        _baseDbRequests.CreateAsync(Arg.Any<Ticket>()).Returns(response);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Name, result.Entity!.Name);
        Assert.Equal(command.Price, result.Entity!.Price);
        Assert.Equal(command.ExpirationDate, result.Entity!.ExpirationDate);
        Assert.Equal(command.ValidityStartDate, result.Entity!.ValidityStartDate);
    }

    [Fact]
    public async Task Handle_WhenCalled_ReturnsNull()
    {
        // Arrange
        var ticketId = Guid.NewGuid();
        var ticket = GetTestTicket(ticketId);

        var command = new CreateTicketHandlerCommand
        {
            Name = ticket.Name,
            Price = ticket.Price,
            ExpirationDate = ticket.ExpirationDate,
            ValidityStartDate = ticket.ValidityStartDate
        };

        var response = new EntityResponse<Ticket>
        {
            Entity = null
        };

        _baseDbRequests.CreateAsync(Arg.Any<Ticket>()).Returns(response);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Null(result.Entity);
    }
}
