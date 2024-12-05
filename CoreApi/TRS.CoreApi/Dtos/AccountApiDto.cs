using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Dtos;

public class AccountApiDto
{
    public Guid AccountId { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    public AccountBalanceApiDto? AccountBalance { get; set; }
}
