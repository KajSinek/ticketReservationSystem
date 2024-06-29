using AppCore.Entities;
using AppCore.Handlers.Tickets;
using Helpers.Responses;
using Helpers.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace TRS.Tests.Handlers.Tickets;

public class GetTicketsHandlerTests : TestBase
{
    private readonly IBaseDbRequests _baseDbRequests;
    private readonly GetTicketsHandler _handler;

    public GetTicketsHandlerTests()
    {
        _baseDbRequests = Substitute.For<IBaseDbRequests>();
        _handler = Substitute.For<GetTicketsHandler>(_baseDbRequests);
    }

    [Fact]
    public async Task Handle_WhenCalled_ReturnsTickets()
    {
        // Arrange
        var ticket = GetTestTicket();
        var ticket2 = GetTestTicket();
        var command = new GetTicketsHandlerQuery();

        var response = new EntitiesResponse<Ticket>
        {
            Entities = new List<Ticket> { ticket, ticket2 }
        };

        _baseDbRequests.GetAllAsync<Ticket>().Returns(response);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }
}
