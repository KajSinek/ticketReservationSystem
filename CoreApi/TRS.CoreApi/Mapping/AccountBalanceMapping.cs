using TRS.CoreApi.Dtos;
using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Mapping;

public static class AccountBalanceMapping
{
    public static AccountBalanceApiDto ToAccountBalanceApiDto(this AccountBalance accountBalance) => new()
    {
        AccountBalanceId = accountBalance.Id,
        Value = accountBalance.Value
    };
}
