using CCA.Sync.Domain.Common;
using CCA.Sync.Domain.ValueObjects;

namespace CCA.Sync.Domain.Tests.ValueObjects;

/// <summary>
/// Tests for the MeterNumber value object.
/// </summary>
public class MeterNumberTests
{
    [Fact]
    public void Create_ValidMeterNumber_ReturnsSuccess()
    {
        // Arrange
        var meterNumber = "METER123456";

        // Act
        var result = MeterNumber.Create(meterNumber);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(meterNumber, result.Value.Value);
    }

    [Fact]
    public void Create_NumericMeterNumber_ReturnsSuccess()
    {
        // Arrange
        var meterNumber = "123456789";

        // Act
        var result = MeterNumber.Create(meterNumber);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_MixedAlphanumeric_ReturnsSuccess()
    {
        // Arrange
        var meterNumber = "MTR1A2B3C4D5E6F7G8H9";

        // Act
        var result = MeterNumber.Create(meterNumber);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_Empty_ReturnsFailure()
    {
        // Arrange
        var meterNumber = "";

        // Act
        var result = MeterNumber.Create(meterNumber);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("MeterNumber.Empty", result.Error.Code);
    }

    [Fact]
    public void Create_Whitespace_ReturnsFailure()
    {
        // Arrange
        var meterNumber = "   ";

        // Act
        var result = MeterNumber.Create(meterNumber);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_TooShort_ReturnsFailure()
    {
        // Arrange
        var meterNumber = "METER";

        // Act
        var result = MeterNumber.Create(meterNumber);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("MeterNumber.InvalidLength", result.Error.Code);
    }

    [Fact]
    public void Create_TooLong_ReturnsFailure()
    {
        // Arrange
        var meterNumber = new string('A', 31);

        // Act
        var result = MeterNumber.Create(meterNumber);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_WithHyphens_ReturnsFailure()
    {
        // Arrange
        var meterNumber = "METER-123456";

        // Act
        var result = MeterNumber.Create(meterNumber);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("MeterNumber.InvalidFormat", result.Error.Code);
    }

    [Fact]
    public void Create_WithSpaces_ReturnsFailure()
    {
        // Arrange
        var meterNumber = "METER 123456";

        // Act
        var result = MeterNumber.Create(meterNumber);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_WithSpecialCharacters_ReturnsFailure()
    {
        // Arrange
        var meterNumber = "METER@123456";

        // Act
        var result = MeterNumber.Create(meterNumber);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_TrimsWhitespace()
    {
        // Arrange
        var meterNumber = "  METER123456  ";

        // Act
        var result = MeterNumber.Create(meterNumber);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("METER123456", result.Value.Value);
    }

    [Fact]
    public void ToString_ReturnsMeterNumberValue()
    {
        // Arrange
        var meterNumber = MeterNumber.Create("METER123456").Value;

        // Act
        var result = meterNumber.ToString();

        // Assert
        Assert.Equal("METER123456", result);
    }

    [Fact]
    public void ImplicitOperator_ToStringConversion()
    {
        // Arrange
        var meterNumber = MeterNumber.Create("METER123456").Value;

        // Act
        string result = meterNumber;

        // Assert
        Assert.Equal("METER123456", result);
    }

    [Fact]
    public void Equals_SameMeterNumber_ReturnsTrue()
    {
        // Arrange
        var meter1 = MeterNumber.Create("METER123456").Value;
        var meter2 = MeterNumber.Create("METER123456").Value;

        // Act & Assert
        Assert.Equal(meter1, meter2);
    }

    [Fact]
    public void Equals_CaseInsensitive_ReturnsTrue()
    {
        // Arrange
        var meter1 = MeterNumber.Create("METER123456").Value;
        var meter2 = MeterNumber.Create("meter123456").Value;

        // Act & Assert
        Assert.Equal(meter1, meter2);
    }

    [Fact]
    public void Equals_DifferentMeterNumber_ReturnsFalse()
    {
        // Arrange
        var meter1 = MeterNumber.Create("METER123456").Value;
        var meter2 = MeterNumber.Create("METER654321").Value;

        // Act & Assert
        Assert.NotEqual(meter1, meter2);
    }

    [Fact]
    public void GetHashCode_SameMeterNumber_SameHashCode()
    {
        // Arrange
        var meter1 = MeterNumber.Create("METER123456").Value;
        var meter2 = MeterNumber.Create("METER123456").Value;

        // Act & Assert
        Assert.Equal(meter1.GetHashCode(), meter2.GetHashCode());
    }

    [Fact]
    public void EqualsOperator_SameMeterNumber_ReturnsTrue()
    {
        // Arrange
        var meter1 = MeterNumber.Create("METER123456").Value;
        var meter2 = MeterNumber.Create("METER123456").Value;

        // Act & Assert
        Assert.True(meter1 == meter2);
    }

    [Fact]
    public void NotEqualsOperator_DifferentMeterNumber_ReturnsTrue()
    {
        // Arrange
        var meter1 = MeterNumber.Create("METER123456").Value;
        var meter2 = MeterNumber.Create("METER654321").Value;

        // Act & Assert
        Assert.True(meter1 != meter2);
    }

    [Theory]
    [InlineData("METER12345")]
    [InlineData("123456")]
    [InlineData("ABCDEF")]
    [InlineData("A1B2C3D4E5")]
    public void Create_ValidLengthNumbers_ReturnsSuccess(string meterNumber)
    {
        // Act
        var result = MeterNumber.Create(meterNumber);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_MaxLengthMeterNumber_ReturnsSuccess()
    {
        // Arrange
        var meterNumber = new string('A', 30);

        // Act
        var result = MeterNumber.Create(meterNumber);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_MinLengthMeterNumber_ReturnsSuccess()
    {
        // Arrange
        var meterNumber = "ABCDEF";

        // Act
        var result = MeterNumber.Create(meterNumber);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
