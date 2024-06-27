using AppCore.Entities;
using AppCore.Handlers.Accounts;
using Helpers.Responses;
using Helpers.Services;
using NSubstitute;

namespace TRS.Tests.Handlers;

public class UpdateAccountHandlerTests : TestBase
{
    private readonly IBaseDbRequests baseDbRequests;
    private readonly UpdateAccountHandler _handler;

    public UpdateAccountHandlerTests()
    {
        baseDbRequests = Substitute.For<IBaseDbRequests>();
        _handler = Substitute.For<UpdateAccountHandler>(baseDbRequests);
    }

    [Fact]
    public async Task Handle_WhenCalled_ReturnsAccount()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var account = GetTestAccount(accountId);
        var updatedAccount = GetTestAccount(accountId, username: "marian");

        var command = new UpdateAccountHandlerCommand
        {
            AccountId = accountId,
            Username = "marian",
            Email = account.Email,
            Address = account.Address,
            FirstName = account.FirstName,
            LastName = account.LastName,
            PhoneNumber = account.PhoneNumber
        };

        var response = new EntityResponse<Account>
        {
            Entity = updatedAccount
        };

        baseDbRequests.UpdateAsync(Arg.Any<Guid>(), Arg.Any<Account>()).Returns(response);

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
    public async Task Handle_WhenUpdateAsyncReturnsNull_ThrowsException()
    {
        // Arrange
        var command = new UpdateAccountHandlerCommand
        {
            AccountId = Guid.NewGuid(),
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

        baseDbRequests.UpdateAsync(Arg.Any<Guid>(), Arg.Any<Account>()).Returns(response);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Entity);
    }
}
