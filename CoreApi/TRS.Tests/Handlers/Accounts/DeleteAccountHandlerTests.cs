using System.Net;
using Helpers.Services;
using NSubstitute;
using TRS.CoreApi.Entities;
using TRS.CoreApi.Handlers.Accounts;
using TRS.CoreApi.Resources;

namespace TRS.Tests.Handlers.Accounts;

public class DeleteAccountHandlerTests : TestBase
{
    private readonly IBaseDbRequests _baseDbRequests;
    private readonly DeleteAccountHandler _handler;

    public DeleteAccountHandlerTests()
    {
        _baseDbRequests = Substitute.For<IBaseDbRequests>();
        _handler = Substitute.For<DeleteAccountHandler>(_baseDbRequests);
    }

    [Fact]
    public async Task Handle_WhenCalled_ReturnsAccount()
    {
        // Arrange
        var accountId = Guid.NewGuid();

        var command = new DeleteAccountHandlerCommand { AccountId = accountId };

        var response = new Response { Errors = new List<string>() };

        _baseDbRequests.DeleteAsync<Account>(Arg.Any<Guid>()).Returns(response);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Handle_WhenGetAsyncReturnsNull_ThrowsException()
    {
        // Arrange
        var query = new DeleteAccountHandlerCommand { AccountId = Guid.NewGuid() };

        var response = new Response
        {
            StatusCode = HttpStatusCode.NotFound,
            Errors = new List<string>
            {
                string.Format(ErrorMessages.EntityNotFound, typeof(Account), query.AccountId)
            }
        };

        _baseDbRequests.DeleteAsync<Account>(Arg.Any<Guid>()).Returns(response);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Contains(
            string.Format(ErrorMessages.EntityNotFound, typeof(Account), query.AccountId),
            result.Errors
        );
    }
}
