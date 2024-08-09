using Helpers.Models;
using Helpers.Responses;
using Helpers.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TRS.CoreApi.Entities;
using TRS.CoreApi.Handlers.Accounts;

namespace TRS.Tests.Handlers.Accounts;

public class AddAccountBalanceHandlerTests : TestBase
{
    public readonly IBaseDbRequests _baseDbRequests;
    public readonly ILogger<AddAccountBalanceHandler> _logger;
    private readonly AddAccountBalanceHandler _handler;

    public AddAccountBalanceHandlerTests()
    {
        _baseDbRequests = Substitute.For<IBaseDbRequests>();
        _logger = Substitute.For<ILogger<AddAccountBalanceHandler>>();
        _handler = new AddAccountBalanceHandler(_baseDbRequests, _logger);
    }

    [Fact]
    public async Task AddAccountBalance_ExpectedResult()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var accountBalanceId = Guid.NewGuid();
        var accountBalance = GetTestAccountBalance(id: accountBalanceId, accountId: accountId);

        var request = new AccountBalanceHandlerCommand { AccountId = accountId, Value = 100 };

        var accountBalanceResponse = new EntityResponse<AccountBalance> { Entity = accountBalance };

        var updatedAccountBalance = new AccountBalance
        {
            Id = accountBalanceId,
            AccountId = accountId,
            Value = accountBalance.Value + request.Value
        };

        var updatedAccountBalanceResponse = new EntityResponse<AccountBalance>
        {
            Entity = updatedAccountBalance
        };

        _baseDbRequests
            .GetEntityByPropertyAsync(Arg.Any<GetEntityByPropertyModel<AccountBalance>>())
            .Returns(accountBalanceResponse);
        _baseDbRequests
            .UpdateAsync(Arg.Any<Guid>(), Arg.Any<AccountBalance>())
            .Returns(updatedAccountBalanceResponse);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(updatedAccountBalanceResponse, result);
    }

    [Fact]
    public async Task AddAccountBalance_ErrorGettingAccountBalance()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var request = new AccountBalanceHandlerCommand { AccountId = accountId, Value = 100 };

        var accountBalanceResponse = new EntityResponse<AccountBalance> { Entity = null };

        _baseDbRequests
            .GetEntityByPropertyAsync(Arg.Any<GetEntityByPropertyModel<AccountBalance>>())
            .Returns(accountBalanceResponse);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(accountBalanceResponse, result);
    }

    [Fact]
    public async Task AddAccountBalance_ErrorUpdatingAccountBalance()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var accountBalanceId = Guid.NewGuid();
        var accountBalance = GetTestAccountBalance(id: accountBalanceId, accountId: accountId);

        var request = new AccountBalanceHandlerCommand { AccountId = accountId, Value = 100 };

        var accountBalanceResponse = new EntityResponse<AccountBalance> { Entity = accountBalance };

        var updatedAccountBalance = new AccountBalance
        {
            Id = accountBalanceId,
            AccountId = accountId,
            Value = accountBalance.Value + request.Value
        };

        var updatedAccountBalanceResponse = new EntityResponse<AccountBalance> { Entity = null };

        _baseDbRequests
            .GetEntityByPropertyAsync(Arg.Any<GetEntityByPropertyModel<AccountBalance>>())
            .Returns(accountBalanceResponse);
        _baseDbRequests
            .UpdateAsync(Arg.Any<Guid>(), Arg.Any<AccountBalance>())
            .Returns(updatedAccountBalanceResponse);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(updatedAccountBalanceResponse, result);
    }
}
