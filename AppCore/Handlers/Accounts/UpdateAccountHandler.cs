using AppCore.Entities;
using Helpers.Responses;
using Helpers.Services;
using MediatR;

namespace AppCore.Handlers.Accounts;

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

public class UpdateAccountHandler(IBaseDbRequests baseDbRequests) : IRequestHandler<UpdateAccountHandlerCommand, EntityResponse<Account>>
{
    public async Task<EntityResponse<Account>> Handle(UpdateAccountHandlerCommand request, CancellationToken cancellationToken)
    {
        var entity = new Account
        {
            AccountId = request.AccountId,
            Username = request.Username,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber
        }; 

        var response = await baseDbRequests.UpdateAsync(request.AccountId, entity);

        return response;
    }
}
