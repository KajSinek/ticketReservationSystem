using AppCore.Entities;

namespace AppCore.Dtos;

public abstract class AccountApiDto(Account account)
{
    public Guid AccountId { get; set; } = account.AccountId;
    public string Username { get; set; } = account.Username;
    public string Email { get; set; } = account.Email;
    public string FirstName { get; set; } = account.FirstName;
    public string LastName { get; set; } = account.LastName;
    public string PhoneNumber { get; set; } = account.PhoneNumber;
    public string Address { get; set; } = account.Address;
}
