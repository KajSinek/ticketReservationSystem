using System.Runtime.Serialization;

namespace TRS.CoreApi.Enums;

public enum AccountTypeEnum
{
    [EnumMember(Value = "Customer")]
    Customer = 0,

    [EnumMember(Value = "Administrator")]
    Administrator = 1
}
