using AppCore.Entities;
using Helpers.Responses;
using Helpers.Services;
using MediatR;

namespace AppCore.Handlers.Accounts;

public class CreateAccountHandlerCommand : IRequest<EntityResponse<Account>>
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Address { get; set; }
    public required string PhoneNumber { get; set; }
}

public class CreateAccountHandler(IBaseDbRequests baseDbRequests, ILogger<CreateAccountHandler> logger) : IRequestHandler<CreateAccountHandlerCommand, EntityResponse<Account>>
{
    public async Task<EntityResponse<Account>> Handle(CreateAccountHandlerCommand request, CancellationToken cancellationToken)
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

        var response = await baseDbRequests.CreateAsync(entity);

        if (response.Entity is null)
        {
            logger.LogError("Failed to create account");
            return response;
        }

        return response;
    }
}
