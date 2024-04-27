namespace AppCore.Entities;

public class User
{
    public required Guid UserId { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
}
