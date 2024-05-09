using AppCore.Database;
using AppCore.Entities;
using MediatR;

namespace AppCore.Handlers.Accounts;

public class CreateAccountHandlerCommand : IRequest<Account>
{
    public required string Username { get; set; }
    public required string Email { get; set; }
}

public class CreateAccountHandler : IRequestHandler<CreateAccountHandlerCommand, Account>
{
    private readonly AppDbContext _context;

    public CreateAccountHandler(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Account> Handle(CreateAccountHandlerCommand request, CancellationToken cancellationToken)
    {
        return new Account { AccountId = Guid.NewGuid(), Address = "Address", Email = request.Email, FirstName = "FirstName", LastName = "LastName", PhoneNumber = "PhoneNumber", Username = request.Username };
    }
}
