using Helpers.Responses;
using Helpers.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Handlers.Accounts;

public class UpdateAccountHandlerCommand : IRequest<EntityResponse<Account>>
{
    public required Guid AccountId { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Address { get; set; }
    public required string PhoneNumber { get; set; }
}

public class UpdateAccountHandler(
    IBaseDbRequests baseDbRequests,
    ILogger<UpdateAccountHandler> logger
) : IRequestHandler<UpdateAccountHandlerCommand, EntityResponse<Account>>
{
    public async Task<EntityResponse<Account>> Handle(
        UpdateAccountHandlerCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new Account
        {
            Id = request.AccountId,
            Username = request.Username,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber
        };

        var accountResponse = await baseDbRequests.UpdateAsync(request.AccountId, entity, query => query.Include(a => a.AccountBalance));

        if (accountResponse.Entity is null)
        {
            logger.LogError("Failed to update Account");
            return accountResponse;
        }

        return accountResponse;
    }
}
