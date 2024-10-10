using TRS.CoreApi.Validators;

namespace TRS.Tests.Validators;

public class CreateAccountApiValidatorTests : TestBase
{
    public CreateAccountApiModelValidator validator;

    public CreateAccountApiValidatorTests()
    {
        validator = new CreateAccountApiModelValidator();
    }

    [Fact]
    public void Account_UsernameIsRequired()
    {
        // Arrange
        var account = GetTestAccountModel(username: string.Empty);

        // Act
        var result = validator.Validate(account);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("Username is required", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Account_UsernameIsLessThan15Characters()
    {
        // Arrange
        var account = GetTestAccountModel(username: "1234567890123456");

        // Act
        var result = validator.Validate(account);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("Username must be less than 15 characters", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Account_EmailIsRequired()
    {
        // Arrange
        var account = GetTestAccountModel(email: string.Empty);

        // Act
        var result = validator.Validate(account);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("Email is required", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Account_FirstNameIsRequired()
    {
        // Arrange
        var account = GetTestAccountModel(firstName: string.Empty);

        // Act
        var result = validator.Validate(account);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("FirstName is required", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Account_FirstNameIsLessThan15Characters()
    {
        // Arrange
        var account = GetTestAccountModel(firstName: "azxcvbnmlkjhgfdsqwertfyuio");

        // Act
        var result = validator.Validate(account);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("FirstName must be less than 25 characters", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Account_LastNameIsRequired()
    {
        // Arrange
        var account = GetTestAccountModel(lastName: string.Empty);

        // Act
        var result = validator.Validate(account);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("LastName is required", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Account_LastNameIsLessThan15Characters()
    {
        // Arrange
        var account = GetTestAccountModel(lastName: "azxcvbnmlkjhgfdsqwerbtyuio");

        // Act
        var result = validator.Validate(account);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("LastName must be less than 25 characters", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Account_AddressIsRequired()
    {
        // Arrange
        var account = GetTestAccountModel(address: string.Empty);

        // Act
        var result = validator.Validate(account);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("Address is required", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Account_AddressContainsNumber()
    {
        // Arrange
        var account = GetTestAccountModel(address: "Elm St");

        // Act
        var result = validator.Validate(account);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("Address must contain a number", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Account_PhoneNumberIsRequired()
    {
        // Arrange
        var account = GetTestAccountModel(phoneNumber: string.Empty);

        // Act
        var result = validator.Validate(account);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("PhoneNumber is required", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Account_PhoneNumberIsLessThan15Characters()
    {
        // Arrange
        var account = GetTestAccountModel(phoneNumber: "1234567890123456");

        // Act
        var result = validator.Validate(account);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal("PhoneNumber must be less than 15 characters", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Account_IsValid()
    {
        // Arrange
        var account = GetTestAccountModel();

        // Act
        var result = validator.Validate(account);

        // Assert
        Assert.True(result.IsValid);
    }
}
