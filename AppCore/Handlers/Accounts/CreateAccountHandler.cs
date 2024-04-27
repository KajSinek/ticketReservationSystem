using AppCore.Database;
using AppCore.Entities;
using AppCore.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Handlers.Accounts;

public class CreateAccountHandlerCommand : IRequest<User>
{
    public required string Username { get; set; }
    public required string Email { get; set; }
}

public class CreateAccountHandler : IRequestHandler<CreateAccountHandlerCommand, User>
{
    private readonly AppDbContext _context;

    public CreateAccountHandler(AppDbContext context)
    {
        _context = context;
    }
    public async Task<User> Handle(CreateAccountHandlerCommand request, CancellationToken cancellationToken)
    {
        var userId = Guid.NewGuid();

        var userEntity = new User
        {
            UserId = userId,
            Username = request.Username,
            Email = request.Email
        };

        var getUser = await _context.Users.FirstOrDefaultAsync();

        if (getUser is not null)
        {
            throw new UserExceptions("User already exists");
        }

        var user = await _context.Users.AddAsync(userEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);



        return userEntity;
    }
}
