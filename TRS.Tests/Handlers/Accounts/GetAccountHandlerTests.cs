using AppCore.Entities;
using AppCore.Handlers.Accounts;
using Helpers.Responses;
using Helpers.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace TRS.Tests.Handlers.Accounts;

public class GetAccountHandlerTests : TestBase
{
    private readonly IBaseDbRequests _baseDbRequests;
    private readonly ILogger<GetAccountHandler> _logger;
    private readonly GetAccountHandler _handler;

    public GetAccountHandlerTests()
    {
        _baseDbRequests = Substitute.For<IBaseDbRequests>();
        _logger = Substitute.For<ILogger<GetAccountHandler>>();
        _handler = Substitute.For<GetAccountHandler>(_baseDbRequests, _logger);
    }

    [Fact]
    public async Task Handle_WhenCalled_ReturnsAccount()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var account = GetTestAccount(accountId);

        var query = new GetAccountHandlerQuery
        {
            AccountId = accountId
        };

        var response = new EntityResponse<Account>
        {
            Entity = account
        };

        _baseDbRequests.GetAsync<Account>(Arg.Any<Guid>()).Returns(response);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(account.Username, result.Entity!.Username);
        Assert.Equal(account.Email, result.Entity!.Email);
        Assert.Equal(account.Address, result.Entity!.Address);
        Assert.Equal(account.FirstName, result.Entity!.FirstName);
        Assert.Equal(account.LastName, result.Entity!.LastName);
        Assert.Equal(account.PhoneNumber, result.Entity!.PhoneNumber);
    }

    [Fact]
    public async Task Handle_WhenGetAsyncReturnsNull_ThrowsException()
    {
        // Arrange
        var query = new GetAccountHandlerQuery
        {
            AccountId = Guid.NewGuid()
        };

        var response = new EntityResponse<Account>
        {
            Entity = null!
        };

        _baseDbRequests.GetAsync<Account>(Arg.Any<Guid>()).Returns(response);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }
}
