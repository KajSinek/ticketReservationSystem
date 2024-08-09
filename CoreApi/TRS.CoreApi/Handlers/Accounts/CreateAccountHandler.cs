using Helpers.Responses;
using Helpers.Services;
using MediatR;
using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Handlers.Accounts;

public class CreateAccountHandlerCommand : IRequest<EntityResponse<Account>>
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Address { get; set; }
    public required string PhoneNumber { get; set; }
}

public class CreateAccountHandler(
    IBaseDbRequests baseDbRequests,
    ILogger<CreateAccountHandler> logger
) : IRequestHandler<CreateAccountHandlerCommand, EntityResponse<Account>>
{
    public async Task<EntityResponse<Account>> Handle(
        CreateAccountHandlerCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new Account
        {
            AccountId = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber
        };

        var accountResponse = await baseDbRequests.CreateAsync(entity);

        if (accountResponse.Entity is null || accountResponse.Entity is not Account account)
        {
            logger.LogError("Failed to create account");
            return accountResponse;
        }

        var accountBalanceEntity = new AccountBalance()
        {
            Id = Guid.NewGuid(),
            AccountId = account.AccountId,
            Value = 0
        };

        var accountBalanceResponse = await baseDbRequests.CreateAsync(accountBalanceEntity);
        accountResponse.Entity.AccountBalance = accountBalanceResponse.Entity;

        if (accountBalanceResponse is null)
        {
            logger.LogError("Failed to create Account Balance");
            return accountResponse;
        }

        return accountResponse;
    }
}
