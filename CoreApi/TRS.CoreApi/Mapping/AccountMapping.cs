using System.Diagnostics.CodeAnalysis;
using TRS.CoreApi.Dtos;
using TRS.CoreApi.Entities;

namespace TRS.CoreApi.Mapping;

[ExcludeFromCodeCoverage]
public static class AccountMapping
{
    public static AccountApiDto ToAccountApiDto(this Account account) => new()
    {
        AccountId = account.Id,
        Username = account.Username,
        Email = account.Email,
        FirstName = account.FirstName,
        LastName = account.LastName,
        PhoneNumber = account.PhoneNumber,
        Address = account.Address,
        AccountBalance = account.AccountBalance?.ToAccountBalanceApiDto()
    };
}
