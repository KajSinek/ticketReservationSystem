using AppCore.Enums;

namespace AppCore.Models;

public class CreateAccountModel
{
    public string Username { get; set; } = null!;
    public string Email { get; set; }

}
