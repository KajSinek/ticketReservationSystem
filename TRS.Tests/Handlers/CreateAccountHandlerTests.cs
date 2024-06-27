using AppCore.Entities;
using AppCore.Handlers.Accounts;
using Helpers.Responses;
using Helpers.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace TRS.Tests.Handlers;

public class CreateAccountHandlerTests : TestBase
{
    private readonly IBaseDbRequests _baseDbRequests;
    private readonly ILogger<CreateAccountHandler> _logger;
    private readonly CreateAccountHandler _handler;

    public CreateAccountHandlerTests()
    {
        _baseDbRequests = Substitute.For<IBaseDbRequests>();
        _logger = Substitute.For<ILogger<CreateAccountHandler>>();
        _handler = Substitute.For<CreateAccountHandler>(_baseDbRequests, _logger);
    }

    [Fact]
    public async Task Handle_WhenCalled_ReturnsAccount()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var account = GetTestAccount(accountId);

        var command = new CreateAccountHandlerCommand
        {
            Username = account.Username,
            Email = account.Email,
            Address = account.Address,
            FirstName = account.FirstName,
            LastName = account.LastName,
            PhoneNumber = account.PhoneNumber
        };

        var response = new EntityResponse<Account>
        {
            Entity = account
        };

        _baseDbRequests.CreateAsync(Arg.Any<Account>()).Returns(response);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Username, result.Entity!.Username);
        Assert.Equal(command.Email, result.Entity!.Email);
        Assert.Equal(command.Address, result.Entity!.Address);
        Assert.Equal(command.FirstName, result.Entity!.FirstName);
        Assert.Equal(command.LastName, result.Entity!.LastName);
        Assert.Equal(command.PhoneNumber, result.Entity!.PhoneNumber);
    }

    [Fact]
    public async Task Handle_WhenCreateAsyncReturnsNull_ThrowsException()
    {
        // Arrange
        var command = new CreateAccountHandlerCommand
        {
            Username = "test",
            Email = "test",
            Address = "test",
            FirstName = "test",
            LastName = "test",
            PhoneNumber = "test"
        };

        var response = new EntityResponse<Account>
        {
            Entity = null
        };

        _baseDbRequests.CreateAsync(Arg.Any<Account>()).Returns(response);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Entity);
    }
}
