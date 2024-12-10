using System.Runtime.Serialization;

namespace TRS.CoreApi.Enums;

public enum AccountBalanceType
{
    [EnumMember(Value = "Add")]
    Add = 0,

    [EnumMember(Value = "Sub")]
    Sub = 10
}
