using Helpers.Responses;
using Helpers.Services;
using MediatR;
using TRS.CoreApi.Entities;
using TRS.CoreApi.Handlers.AccountBalances;

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
    IMediator mediator,
    ILogger<CreateAccountHandler> logger
) : IRequestHandler<CreateAccountHandlerCommand, EntityResponse<Account>>
{
    public async Task<EntityResponse<Account>> Handle(
        CreateAccountHandlerCommand request,
        CancellationToken ct
    )
    {
        var entity = new Account
        {
            Id = Guid.NewGuid(),
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

        var accountBalanceResponse = await mediator.Send(new CreateAccountBalanceCommand
        {
            AccountId = account.Id
        }, ct);

        if (accountBalanceResponse.Entity is null || accountBalanceResponse.Entity is not AccountBalance accountBalance)
        {
            logger.LogError("Failed to create Account Balance");
            return accountResponse;
        }

        var updateAccountResponse = await baseDbRequests.UpdateAsync(account.Id, account);

        if (updateAccountResponse.IsFailure || updateAccountResponse.Entity is not Account updatedAccount)
        {
            logger.LogError("Failed to update account");
            return updateAccountResponse;
        }

        updatedAccount.AccountBalance = accountBalance;

        return updateAccountResponse;
    }
}
