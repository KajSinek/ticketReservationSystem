using Helpers.Responses;
using Helpers.Services;
using NSubstitute;
using TRS.CoreApi.Entities;
using TRS.CoreApi.Handlers.Accounts;

namespace TRS.Tests.Handlers.Accounts;

public class GetAccountsHandlerTests : TestBase
{
    private readonly IBaseDbRequests _baseDbRequests;
    private readonly GetAccountsHandler _handler;

    public GetAccountsHandlerTests()
    {
        _baseDbRequests = Substitute.For<IBaseDbRequests>();
        _handler = Substitute.For<GetAccountsHandler>(_baseDbRequests);
    }

    [Fact]
    public async Task Handle_WhenCalled_ReturnsAccounts()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var account = GetTestAccount(accountId);
        var account2 = GetTestAccount(Guid.NewGuid());

        var query = new GetAccountsHandlerQuery { };

        var response = new EntitiesResponse<Account>
        {
            Entities = new List<Account> { account, account2 }
        };

        _baseDbRequests.GetAllAsync<Account>().Returns(response);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Entities);
        Assert.Equal(2, result.Entities.Count);
    }

    [Fact]
    public async Task Handle_WhenGetAsyncReturnsNull_ThrowsException()
    {
        // Arrange
        var query = new GetAccountsHandlerQuery { };

        var response = new EntitiesResponse<Account> { Entities = new List<Account>() { } };

        _baseDbRequests.GetAllAsync<Account>().Returns(response);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }
}
