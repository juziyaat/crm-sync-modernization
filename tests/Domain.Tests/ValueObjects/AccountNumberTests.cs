using CCA.Sync.Domain.Common;
using CCA.Sync.Domain.ValueObjects;

namespace CCA.Sync.Domain.Tests.ValueObjects;

/// <summary>
/// Tests for the AccountNumber value object.
/// </summary>
public class AccountNumberTests
{
    [Fact]
    public void Create_ValidAccountNumber_ReturnsSuccess()
    {
        // Arrange
        var accountNumber = "ACC123456";

        // Act
        var result = AccountNumber.Create(accountNumber);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(accountNumber, result.Value.Value);
    }

    [Fact]
    public void Create_AccountNumberWithHyphens_ReturnsSuccess()
    {
        // Arrange
        var accountNumber = "ACC-123-456";

        // Act
        var result = AccountNumber.Create(accountNumber);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_AccountNumberWithUnderscores_ReturnsSuccess()
    {
        // Arrange
        var accountNumber = "ACC_123_456";

        // Act
        var result = AccountNumber.Create(accountNumber);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_NumericAccountNumber_ReturnsSuccess()
    {
        // Arrange
        var accountNumber = "123456789";

        // Act
        var result = AccountNumber.Create(accountNumber);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_Empty_ReturnsFailure()
    {
        // Arrange
        var accountNumber = "";

        // Act
        var result = AccountNumber.Create(accountNumber);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("AccountNumber.Empty", result.Error.Code);
    }

    [Fact]
    public void Create_Whitespace_ReturnsFailure()
    {
        // Arrange
        var accountNumber = "   ";

        // Act
        var result = AccountNumber.Create(accountNumber);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_TooShort_ReturnsFailure()
    {
        // Arrange
        var accountNumber = "AB";

        // Act
        var result = AccountNumber.Create(accountNumber);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("AccountNumber.InvalidLength", result.Error.Code);
    }

    [Fact]
    public void Create_TooLong_ReturnsFailure()
    {
        // Arrange
        var accountNumber = new string('A', 51);

        // Act
        var result = AccountNumber.Create(accountNumber);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_WithSpaces_ReturnsFailure()
    {
        // Arrange
        var accountNumber = "ACC 123456";

        // Act
        var result = AccountNumber.Create(accountNumber);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("AccountNumber.InvalidFormat", result.Error.Code);
    }

    [Fact]
    public void Create_WithSpecialCharacters_ReturnsFailure()
    {
        // Arrange
        var accountNumber = "ACC@123456";

        // Act
        var result = AccountNumber.Create(accountNumber);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_TrimsWhitespace()
    {
        // Arrange
        var accountNumber = "  ACC123456  ";

        // Act
        var result = AccountNumber.Create(accountNumber);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("ACC123456", result.Value.Value);
    }

    [Fact]
    public void ToString_ReturnsAccountNumberValue()
    {
        // Arrange
        var accountNumber = AccountNumber.Create("ACC123456").Value;

        // Act
        var result = accountNumber.ToString();

        // Assert
        Assert.Equal("ACC123456", result);
    }

    [Fact]
    public void ImplicitOperator_ToStringConversion()
    {
        // Arrange
        var accountNumber = AccountNumber.Create("ACC123456").Value;

        // Act
        string result = accountNumber;

        // Assert
        Assert.Equal("ACC123456", result);
    }

    [Fact]
    public void Equals_SameAccountNumber_ReturnsTrue()
    {
        // Arrange
        var account1 = AccountNumber.Create("ACC123456").Value;
        var account2 = AccountNumber.Create("ACC123456").Value;

        // Act & Assert
        Assert.Equal(account1, account2);
    }

    [Fact]
    public void Equals_CaseInsensitive_ReturnsTrue()
    {
        // Arrange
        var account1 = AccountNumber.Create("ACC123456").Value;
        var account2 = AccountNumber.Create("acc123456").Value;

        // Act & Assert
        Assert.Equal(account1, account2);
    }

    [Fact]
    public void Equals_DifferentAccountNumber_ReturnsFalse()
    {
        // Arrange
        var account1 = AccountNumber.Create("ACC123456").Value;
        var account2 = AccountNumber.Create("ACC654321").Value;

        // Act & Assert
        Assert.NotEqual(account1, account2);
    }

    [Fact]
    public void GetHashCode_SameAccountNumber_SameHashCode()
    {
        // Arrange
        var account1 = AccountNumber.Create("ACC123456").Value;
        var account2 = AccountNumber.Create("ACC123456").Value;

        // Act & Assert
        Assert.Equal(account1.GetHashCode(), account2.GetHashCode());
    }

    [Fact]
    public void EqualsOperator_SameAccountNumber_ReturnsTrue()
    {
        // Arrange
        var account1 = AccountNumber.Create("ACC123456").Value;
        var account2 = AccountNumber.Create("ACC123456").Value;

        // Act & Assert
        Assert.True(account1 == account2);
    }

    [Fact]
    public void NotEqualsOperator_DifferentAccountNumber_ReturnsTrue()
    {
        // Arrange
        var account1 = AccountNumber.Create("ACC123456").Value;
        var account2 = AccountNumber.Create("ACC654321").Value;

        // Act & Assert
        Assert.True(account1 != account2);
    }

    [Theory]
    [InlineData("ABC")]
    [InlineData("123")]
    [InlineData("ABC-123")]
    [InlineData("ABC_123")]
    [InlineData("ABC-123_DEF")]
    public void Create_ValidFormats_ReturnsSuccess(string accountNumber)
    {
        // Act
        var result = AccountNumber.Create(accountNumber);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData("ABC#123")]
    [InlineData("ABC@123")]
    [InlineData("ABC!123")]
    public void Create_InvalidCharacters_ReturnsFailure(string accountNumber)
    {
        // Act
        var result = AccountNumber.Create(accountNumber);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_MaxLengthAccountNumber_ReturnsSuccess()
    {
        // Arrange
        var accountNumber = new string('A', 50);

        // Act
        var result = AccountNumber.Create(accountNumber);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_MinLengthAccountNumber_ReturnsSuccess()
    {
        // Arrange
        var accountNumber = "ABC";

        // Act
        var result = AccountNumber.Create(accountNumber);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
