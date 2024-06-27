using AppCore.Entities;
using AppCore.Models;

namespace TRS.Tests;

public class TestBase
{
    public static CreateAccountModel GetTestAccountModel(string? username = null,
                                                         string? email = null,
                                                         string? firstName = null,
                                                         string? lastName = null,
                                                         string? phoneNumber = null,
                                                         string? address = null)
    {
        return new CreateAccountModel
        {
            Username = username ?? "test",
            Email = email ?? "test@test.sk",
            FirstName = firstName ?? "John",
            LastName = lastName ?? "Doe",
            PhoneNumber = phoneNumber ?? "123456789",
            Address = address ?? "1234 Elm St"
        };
    }

    public static Account GetTestAccount(Guid? accountId = null,
                                         string? username = null,
                                         string? email = null,
                                         string? firstName = null,
                                         string? lastName = null,
                                         string? phoneNumber = null,
                                         string? address = null)
    {
        return new Account
        {
            AccountId = accountId ?? Guid.NewGuid(),
            Username = username ?? "test",
            Email = email ?? "test@test.com",
            FirstName = firstName ?? "John",
            LastName = lastName ?? "Doe",
            PhoneNumber = phoneNumber ?? "123456789",
            Address = address ?? "1234 Elm St"
        };
    }
}