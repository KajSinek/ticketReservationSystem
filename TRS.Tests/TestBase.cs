using AppCore.Entities;
using AppCore.Models;
using Helpers.Utilities;

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

    public static Ticket GetTestTicket(Guid? ticketId = null,
                                       string? name = null,
                                       decimal? price = null,
                                       DateOnly? ExpirationDate = null,
                                       DateOnly? ValidityStartDate = null)
    {
        return new Ticket
        {
            TicketId = ticketId ?? Guid.NewGuid(),
            Name = name ?? "Test Ticket",
            Price = price ?? 10.0m,
            ExpirationDate = ExpirationDate ?? DateHelper.DateOnlyUtcNow,
            ValidityStartDate = ValidityStartDate ?? DateHelper.DateOnlyUtcNow.AddDays(1)
        };
    }

    public static AccountBalance GetTestAccountBalance(Guid? id = null,
                                                Guid? accountId = null,
                                                decimal? value = null)
    {
        return new AccountBalance
        {
            Id = id ?? Guid.NewGuid(),
            AccountId = accountId ?? Guid.NewGuid(),
            Value = value ?? 0
        };
    }
}