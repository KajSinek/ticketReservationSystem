namespace TRS.CoreApi.Entities;

public class Account
{
    public required Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Address { get; set; }
    public required string PhoneNumber { get; set; }

    public Guid? AccountBalanceId { get; set; }
    public virtual AccountBalance? AccountBalance { get; set; }
    public Guid? AccountBasketId { get; set; }
    public virtual AccountBasket? AccountBasket { get; set; }
}
