using TRS.CoreApi.Enums;

namespace TRS.CoreApi.Models;

public class UpdateAccountBalanceModel
{
    public required decimal Value { get; set; }
    public required AccountBalanceType Type { get; set; }
}
