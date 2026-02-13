using CCA.Sync.Domain.Common;
using CCA.Sync.Domain.ValueObjects;

namespace CCA.Sync.Domain.Tests.ValueObjects;

/// <summary>
/// Tests for the PhoneNumber value object.
/// </summary>
public class PhoneNumberTests
{
    [Fact]
    public void Create_PlainTenDigits_ReturnsSuccess()
    {
        // Arrange
        var phone = "2234567890";

        // Act
        var result = PhoneNumber.Create(phone);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("2234567890", result.Value.Value);
    }

    [Fact]
    public void Create_FormattedWithParentheses_ReturnsSuccess()
    {
        // Arrange
        var phone = "(223) 456-7890";

        // Act
        var result = PhoneNumber.Create(phone);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("2234567890", result.Value.Value);
    }

    [Fact]
    public void Create_FormattedWithHyphens_ReturnsSuccess()
    {
        // Arrange
        var phone = "223-456-7890";

        // Act
        var result = PhoneNumber.Create(phone);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("2234567890", result.Value.Value);
    }

    [Fact]
    public void Create_FormattedWithDots_ReturnsSuccess()
    {
        // Arrange
        var phone = "223.456.7890";

        // Act
        var result = PhoneNumber.Create(phone);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_WithCountryCode_ReturnsSuccess()
    {
        // Arrange
        var phone = "+1-223-456-7890";

        // Act
        var result = PhoneNumber.Create(phone);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("2234567890", result.Value.Value);
    }

    [Fact]
    public void Create_WithCountryCodeNoHyphen_ReturnsSuccess()
    {
        // Arrange
        var phone = "+12234567890";

        // Act
        var result = PhoneNumber.Create(phone);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_WithSpaces_ReturnsSuccess()
    {
        // Arrange
        var phone = "223 456 7890";

        // Act
        var result = PhoneNumber.Create(phone);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_Empty_ReturnsFailure()
    {
        // Arrange
        var phone = "";

        // Act
        var result = PhoneNumber.Create(phone);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("PhoneNumber.Empty", result.Error.Code);
    }

    [Fact]
    public void Create_Whitespace_ReturnsFailure()
    {
        // Arrange
        var phone = "   ";

        // Act
        var result = PhoneNumber.Create(phone);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_TooFewDigits_ReturnsFailure()
    {
        // Arrange
        var phone = "223456789"; // 9 digits

        // Act
        var result = PhoneNumber.Create(phone);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("PhoneNumber.Invalid", result.Error.Code);
    }

    [Fact]
    public void Create_TooManyDigits_ReturnsFailure()
    {
        // Arrange
        var phone = "22345678901234"; // More than 11 digits

        // Act
        var result = PhoneNumber.Create(phone);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_InvalidFormat_ReturnsFailure()
    {
        // Arrange
        var phone = "abc-def-ghij";

        // Act
        var result = PhoneNumber.Create(phone);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_ZeroAreaCode_ReturnsFailure()
    {
        // Arrange
        var phone = "0234567890";

        // Act
        var result = PhoneNumber.Create(phone);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_OneAsFirstDigit_ReturnsFailure()
    {
        // Arrange
        var phone = "1134567890";

        // Act
        var result = PhoneNumber.Create(phone);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void FormatDisplay_ReturnsParenthesesFormat()
    {
        // Arrange
        var phone = PhoneNumber.Create("2234567890").Value;

        // Act
        var result = phone.FormatDisplay();

        // Assert
        Assert.Equal("(223) 456-7890", result);
    }

    [Fact]
    public void FormatE164_ReturnsE164Format()
    {
        // Arrange
        var phone = PhoneNumber.Create("2234567890").Value;

        // Act
        var result = phone.FormatE164();

        // Assert
        Assert.Equal("+12234567890", result);
    }

    [Fact]
    public void ToString_ReturnsDisplayFormat()
    {
        // Arrange
        var phone = PhoneNumber.Create("2234567890").Value;

        // Act
        var result = phone.ToString();

        // Assert
        Assert.Equal("(223) 456-7890", result);
    }

    [Fact]
    public void Equals_SamePhone_ReturnsTrue()
    {
        // Arrange
        var phone1 = PhoneNumber.Create("2234567890").Value;
        var phone2 = PhoneNumber.Create("(223) 456-7890").Value;

        // Act & Assert
        Assert.Equal(phone1, phone2);
    }

    [Fact]
    public void Equals_DifferentPhone_ReturnsFalse()
    {
        // Arrange
        var phone1 = PhoneNumber.Create("2234567890").Value;
        var phone2 = PhoneNumber.Create("3334567890").Value;

        // Act & Assert
        Assert.NotEqual(phone1, phone2);
    }

    [Fact]
    public void GetHashCode_SamePhone_SameHashCode()
    {
        // Arrange
        var phone1 = PhoneNumber.Create("2234567890").Value;
        var phone2 = PhoneNumber.Create("(223) 456-7890").Value;

        // Act & Assert
        Assert.Equal(phone1.GetHashCode(), phone2.GetHashCode());
    }

    [Fact]
    public void ImplicitOperator_ToStringConversion()
    {
        // Arrange
        var phone = PhoneNumber.Create("2234567890").Value;

        // Act
        string result = phone;

        // Assert
        Assert.Equal("2234567890", result);
    }

    [Fact]
    public void EqualsOperator_SamePhone_ReturnsTrue()
    {
        // Arrange
        var phone1 = PhoneNumber.Create("2234567890").Value;
        var phone2 = PhoneNumber.Create("2234567890").Value;

        // Act & Assert
        Assert.True(phone1 == phone2);
    }

    [Fact]
    public void NotEqualsOperator_DifferentPhone_ReturnsTrue()
    {
        // Arrange
        var phone1 = PhoneNumber.Create("2234567890").Value;
        var phone2 = PhoneNumber.Create("3334567890").Value;

        // Act & Assert
        Assert.True(phone1 != phone2);
    }

    [Fact]
    public void Create_WithLeadingTrailingWhitespace_Trims()
    {
        // Arrange
        var phone = "  223-456-7890  ";

        // Act
        var result = PhoneNumber.Create(phone);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("2234567890", result.Value.Value);
    }

    [Theory]
    [InlineData("2234567890")]
    [InlineData("3334567890")]
    [InlineData("9994567890")]
    public void Create_ValidAreaCodes_ReturnsSuccess(string phone)
    {
        // Act
        var result = PhoneNumber.Create(phone);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData("2234567890")]
    [InlineData("2334567890")]
    [InlineData("2934567890")]
    public void Create_ValidExchangeCodes_ReturnsSuccess(string phone)
    {
        // Act
        var result = PhoneNumber.Create(phone);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
