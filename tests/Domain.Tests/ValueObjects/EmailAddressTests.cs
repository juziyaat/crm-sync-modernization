using CCA.Sync.Domain.Common;
using CCA.Sync.Domain.ValueObjects;

namespace CCA.Sync.Domain.Tests.ValueObjects;

/// <summary>
/// Tests for the EmailAddress value object.
/// </summary>
public class EmailAddressTests
{
    [Fact]
    public void Create_ValidEmail_ReturnsSuccess()
    {
        // Arrange
        var email = "user@example.com";

        // Act
        var result = EmailAddress.Create(email);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(email, result.Value.Value);
    }

    [Fact]
    public void Create_ValidEmailWithPlus_ReturnsSuccess()
    {
        // Arrange
        var email = "user+tag@example.com";

        // Act
        var result = EmailAddress.Create(email);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(email, result.Value.Value);
    }

    [Fact]
    public void Create_ValidEmailWithNumbers_ReturnsSuccess()
    {
        // Arrange
        var email = "user123@example456.com";

        // Act
        var result = EmailAddress.Create(email);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_ValidEmailWithSubdomain_ReturnsSuccess()
    {
        // Arrange
        var email = "user@mail.example.co.uk";

        // Act
        var result = EmailAddress.Create(email);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_EmptyString_ReturnsFailure()
    {
        // Arrange
        var email = "";

        // Act
        var result = EmailAddress.Create(email);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("EmailAddress.Empty", result.Error.Code);
    }

    [Fact]
    public void Create_Whitespace_ReturnsFailure()
    {
        // Arrange
        var email = "   ";

        // Act
        var result = EmailAddress.Create(email);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_NoAtSign_ReturnsFailure()
    {
        // Arrange
        var email = "userexample.com";

        // Act
        var result = EmailAddress.Create(email);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("EmailAddress.Invalid", result.Error.Code);
    }

    [Fact]
    public void Create_MissingLocalPart_ReturnsFailure()
    {
        // Arrange
        var email = "@example.com";

        // Act
        var result = EmailAddress.Create(email);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_MissingDomain_ReturnsFailure()
    {
        // Arrange
        var email = "user@";

        // Act
        var result = EmailAddress.Create(email);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_TooLong_ReturnsFailure()
    {
        // Arrange
        var email = new string('a', 250) + "@example.com";

        // Act
        var result = EmailAddress.Create(email);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("EmailAddress.TooLong", result.Error.Code);
    }

    [Fact]
    public void Create_WithLeadingTrailingWhitespace_Trims()
    {
        // Arrange
        var email = "  user@example.com  ";

        // Act
        var result = EmailAddress.Create(email);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("user@example.com", result.Value.Value);
    }

    [Fact]
    public void Create_PreservesCase()
    {
        // Arrange
        var email = "User@Example.COM";

        // Act
        var result = EmailAddress.Create(email);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("User@Example.COM", result.Value.Value);
    }

    [Fact]
    public void Equals_SameEmail_ReturnsTrue()
    {
        // Arrange
        var email1 = EmailAddress.Create("user@example.com").Value;
        var email2 = EmailAddress.Create("user@example.com").Value;

        // Act & Assert
        Assert.Equal(email1, email2);
    }

    [Fact]
    public void Equals_DifferentCase_ReturnsTrue()
    {
        // Arrange
        var email1 = EmailAddress.Create("user@example.com").Value;
        var email2 = EmailAddress.Create("USER@EXAMPLE.COM").Value;

        // Act & Assert
        Assert.Equal(email1, email2);
    }

    [Fact]
    public void Equals_DifferentEmail_ReturnsFalse()
    {
        // Arrange
        var email1 = EmailAddress.Create("user1@example.com").Value;
        var email2 = EmailAddress.Create("user2@example.com").Value;

        // Act & Assert
        Assert.NotEqual(email1, email2);
    }

    [Fact]
    public void GetHashCode_SameEmail_SameHashCode()
    {
        // Arrange
        var email1 = EmailAddress.Create("user@example.com").Value;
        var email2 = EmailAddress.Create("user@example.com").Value;

        // Act & Assert
        Assert.Equal(email1.GetHashCode(), email2.GetHashCode());
    }

    [Fact]
    public void ImplicitOperator_ToStringConversion()
    {
        // Arrange
        var email = EmailAddress.Create("user@example.com").Value;

        // Act
        string result = email;

        // Assert
        Assert.Equal("user@example.com", result);
    }

    [Fact]
    public void ToString_ReturnsEmailValue()
    {
        // Arrange
        var email = EmailAddress.Create("user@example.com").Value;

        // Act
        var result = email.ToString();

        // Assert
        Assert.Equal("user@example.com", result);
    }

    [Fact]
    public void EqualsOperator_SameEmail_ReturnsTrue()
    {
        // Arrange
        var email1 = EmailAddress.Create("user@example.com").Value;
        var email2 = EmailAddress.Create("user@example.com").Value;

        // Act & Assert
        Assert.True(email1 == email2);
    }

    [Fact]
    public void NotEqualsOperator_DifferentEmail_ReturnsTrue()
    {
        // Arrange
        var email1 = EmailAddress.Create("user1@example.com").Value;
        var email2 = EmailAddress.Create("user2@example.com").Value;

        // Act & Assert
        Assert.True(email1 != email2);
    }

    [Theory]
    [InlineData("test.email@example.com")]
    [InlineData("test_email@example.com")]
    [InlineData("test-email@example.com")]
    [InlineData("test123@example.com")]
    public void Create_ValidSpecialCharacters_ReturnsSuccess(string email)
    {
        // Act
        var result = EmailAddress.Create(email);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData("test@domain")]
    [InlineData("test@@example.com")]
    [InlineData("test@.com")]
    [InlineData("test@example..com")]
    public void Create_InvalidFormats_ReturnsFailure(string email)
    {
        // Act
        var result = EmailAddress.Create(email);

        // Assert
        Assert.True(result.IsFailure);
    }
}
