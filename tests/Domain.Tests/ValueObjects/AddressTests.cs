using CCA.Sync.Domain.Common;
using CCA.Sync.Domain.ValueObjects;

namespace CCA.Sync.Domain.Tests.ValueObjects;

/// <summary>
/// Tests for the Address value object.
/// </summary>
public class AddressTests
{
    [Fact]
    public void Create_ValidAddress_ReturnsSuccess()
    {
        // Arrange
        var street = "123 Main St";
        var city = "San Francisco";
        var state = "CA";
        var zipCode = "94102";

        // Act
        var result = Address.Create(street, city, state, zipCode);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(street, result.Value.Street);
        Assert.Equal(city, result.Value.City);
        Assert.Equal(state, result.Value.State);
        Assert.Equal(zipCode, result.Value.ZipCode);
    }

    [Fact]
    public void Create_ValidAddressWithExtendedZipCode_ReturnsSuccess()
    {
        // Arrange
        var street = "123 Main St";
        var city = "San Francisco";
        var state = "CA";
        var zipCode = "94102-1234";

        // Act
        var result = Address.Create(street, city, state, zipCode);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(zipCode, result.Value.ZipCode);
    }

    [Fact]
    public void Create_EmptyStreet_ReturnsFailure()
    {
        // Arrange
        var street = "";
        var city = "San Francisco";
        var state = "CA";
        var zipCode = "94102";

        // Act
        var result = Address.Create(street, city, state, zipCode);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Address.StreetEmpty", result.Error.Code);
    }

    [Fact]
    public void Create_EmptyCity_ReturnsFailure()
    {
        // Arrange
        var street = "123 Main St";
        var city = "";
        var state = "CA";
        var zipCode = "94102";

        // Act
        var result = Address.Create(street, city, state, zipCode);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Address.CityEmpty", result.Error.Code);
    }

    [Fact]
    public void Create_EmptyState_ReturnsFailure()
    {
        // Arrange
        var street = "123 Main St";
        var city = "San Francisco";
        var state = "";
        var zipCode = "94102";

        // Act
        var result = Address.Create(street, city, state, zipCode);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Address.StateEmpty", result.Error.Code);
    }

    [Fact]
    public void Create_EmptyZipCode_ReturnsFailure()
    {
        // Arrange
        var street = "123 Main St";
        var city = "San Francisco";
        var state = "CA";
        var zipCode = "";

        // Act
        var result = Address.Create(street, city, state, zipCode);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Address.ZipCodeEmpty", result.Error.Code);
    }

    [Fact]
    public void Create_InvalidState_ReturnsFailure()
    {
        // Arrange
        var street = "123 Main St";
        var city = "San Francisco";
        var state = "XX";
        var zipCode = "94102";

        // Act
        var result = Address.Create(street, city, state, zipCode);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Address.StateInvalid", result.Error.Code);
    }

    [Fact]
    public void Create_StateTooShort_ReturnsFailure()
    {
        // Arrange
        var street = "123 Main St";
        var city = "San Francisco";
        var state = "C";
        var zipCode = "94102";

        // Act
        var result = Address.Create(street, city, state, zipCode);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_InvalidZipCode_ReturnsFailure()
    {
        // Arrange
        var street = "123 Main St";
        var city = "San Francisco";
        var state = "CA";
        var zipCode = "9410";

        // Act
        var result = Address.Create(street, city, state, zipCode);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Address.ZipCodeInvalid", result.Error.Code);
    }

    [Fact]
    public void Create_ZipCodeWithLetters_ReturnsFailure()
    {
        // Arrange
        var street = "123 Main St";
        var city = "San Francisco";
        var state = "CA";
        var zipCode = "9410A";

        // Act
        var result = Address.Create(street, city, state, zipCode);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_StateNormalizesToUppercase()
    {
        // Arrange
        var street = "123 Main St";
        var city = "San Francisco";
        var state = "ca";
        var zipCode = "94102";

        // Act
        var result = Address.Create(street, city, state, zipCode);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("CA", result.Value.State);
    }

    [Fact]
    public void Create_TrimsWhitespace()
    {
        // Arrange
        var street = "  123 Main St  ";
        var city = "  San Francisco  ";
        var state = "  CA  ";
        var zipCode = "  94102  ";

        // Act
        var result = Address.Create(street, city, state, zipCode);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("123 Main St", result.Value.Street);
        Assert.Equal("San Francisco", result.Value.City);
        Assert.Equal("CA", result.Value.State);
        Assert.Equal("94102", result.Value.ZipCode);
    }

    [Fact]
    public void ToString_FormatsAddress()
    {
        // Arrange
        var address = Address.Create("123 Main St", "San Francisco", "CA", "94102").Value;

        // Act
        var result = address.ToString();

        // Assert
        Assert.Equal("123 Main St, San Francisco, CA 94102", result);
    }

    [Fact]
    public void Equals_SameAddress_ReturnsTrue()
    {
        // Arrange
        var address1 = Address.Create("123 Main St", "San Francisco", "CA", "94102").Value;
        var address2 = Address.Create("123 Main St", "San Francisco", "CA", "94102").Value;

        // Act & Assert
        Assert.Equal(address1, address2);
    }

    [Fact]
    public void Equals_DifferentStreet_ReturnsFalse()
    {
        // Arrange
        var address1 = Address.Create("123 Main St", "San Francisco", "CA", "94102").Value;
        var address2 = Address.Create("456 Oak Ave", "San Francisco", "CA", "94102").Value;

        // Act & Assert
        Assert.NotEqual(address1, address2);
    }

    [Fact]
    public void Equals_DifferentCity_ReturnsFalse()
    {
        // Arrange
        var address1 = Address.Create("123 Main St", "San Francisco", "CA", "94102").Value;
        var address2 = Address.Create("123 Main St", "Oakland", "CA", "94102").Value;

        // Act & Assert
        Assert.NotEqual(address1, address2);
    }

    [Fact]
    public void Equals_DifferentState_ReturnsFalse()
    {
        // Arrange
        var address1 = Address.Create("123 Main St", "San Francisco", "CA", "94102").Value;
        var address2 = Address.Create("123 Main St", "San Francisco", "TX", "94102").Value;

        // Act & Assert
        Assert.NotEqual(address1, address2);
    }

    [Fact]
    public void Equals_DifferentZipCode_ReturnsFalse()
    {
        // Arrange
        var address1 = Address.Create("123 Main St", "San Francisco", "CA", "94102").Value;
        var address2 = Address.Create("123 Main St", "San Francisco", "CA", "94103").Value;

        // Act & Assert
        Assert.NotEqual(address1, address2);
    }

    [Fact]
    public void Equals_CaseInsensitiveCity_ReturnsTrue()
    {
        // Arrange
        var address1 = Address.Create("123 Main St", "San Francisco", "CA", "94102").Value;
        var address2 = Address.Create("123 Main St", "SAN FRANCISCO", "CA", "94102").Value;

        // Act & Assert
        Assert.Equal(address1, address2);
    }

    [Fact]
    public void GetHashCode_SameAddress_SameHashCode()
    {
        // Arrange
        var address1 = Address.Create("123 Main St", "San Francisco", "CA", "94102").Value;
        var address2 = Address.Create("123 Main St", "San Francisco", "CA", "94102").Value;

        // Act & Assert
        Assert.Equal(address1.GetHashCode(), address2.GetHashCode());
    }

    [Fact]
    public void EqualsOperator_SameAddress_ReturnsTrue()
    {
        // Arrange
        var address1 = Address.Create("123 Main St", "San Francisco", "CA", "94102").Value;
        var address2 = Address.Create("123 Main St", "San Francisco", "CA", "94102").Value;

        // Act & Assert
        Assert.True(address1 == address2);
    }

    [Fact]
    public void NotEqualsOperator_DifferentAddress_ReturnsTrue()
    {
        // Arrange
        var address1 = Address.Create("123 Main St", "San Francisco", "CA", "94102").Value;
        var address2 = Address.Create("456 Oak Ave", "Oakland", "CA", "94606").Value;

        // Act & Assert
        Assert.True(address1 != address2);
    }

    [Theory]
    [InlineData("CA")]
    [InlineData("TX")]
    [InlineData("NY")]
    [InlineData("FL")]
    [InlineData("DC")]
    public void Create_ValidUSStates_ReturnsSuccess(string state)
    {
        // Act
        var result = Address.Create("123 Main St", "City", state, "12345");

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("12345-6789")]
    public void Create_ValidZipCodeFormats_ReturnsSuccess(string zipCode)
    {
        // Act
        var result = Address.Create("123 Main St", "City", "CA", zipCode);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
