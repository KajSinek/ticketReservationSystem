using AppCore.Entities;

namespace AppCore.Dtos;

public abstract class AccountApiDto
{
    protected AccountApiDto(Account account)
    {
        AccountId = account.AccountId;
        Username = account.Username;
        Email = account.Email;
        FirstName = account.FirstName;
        LastName = account.LastName;
        PhoneNumber = account.PhoneNumber;
        Address = account.Address;
    }
    public Guid AccountId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}
