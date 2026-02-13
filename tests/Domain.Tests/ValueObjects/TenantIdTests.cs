using CCA.Sync.Domain.Common;
using CCA.Sync.Domain.ValueObjects;

namespace CCA.Sync.Domain.Tests.ValueObjects;

/// <summary>
/// Tests for the TenantId value object.
/// </summary>
public class TenantIdTests
{
    [Fact]
    public void Create_ValidGuid_ReturnsSuccess()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var result = TenantId.Create(guid);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(guid, result.Value.Value);
    }

    [Fact]
    public void Create_EmptyGuid_ReturnsFailure()
    {
        // Arrange
        var guid = Guid.Empty;

        // Act
        var result = TenantId.Create(guid);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("TenantId.Empty", result.Error.Code);
    }

    [Fact]
    public void Create_ValidGuidString_ReturnsSuccess()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var guidString = guid.ToString();

        // Act
        var result = TenantId.Create(guidString);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(guid, result.Value.Value);
    }

    [Fact]
    public void Create_GuidStringWithBraces_ReturnsSuccess()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var guidString = $"{{{guid}}}";

        // Act
        var result = TenantId.Create(guidString);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_GuidStringUppercase_ReturnsSuccess()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var guidString = guid.ToString().ToUpperInvariant();

        // Act
        var result = TenantId.Create(guidString);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_EmptyString_ReturnsFailure()
    {
        // Arrange
        var guidString = "";

        // Act
        var result = TenantId.Create(guidString);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("TenantId.Empty", result.Error.Code);
    }

    [Fact]
    public void Create_Whitespace_ReturnsFailure()
    {
        // Arrange
        var guidString = "   ";

        // Act
        var result = TenantId.Create(guidString);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_InvalidGuidString_ReturnsFailure()
    {
        // Arrange
        var guidString = "not-a-guid";

        // Act
        var result = TenantId.Create(guidString);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("TenantId.InvalidFormat", result.Error.Code);
    }

    [Fact]
    public void Create_EmptyGuidString_ReturnsFailure()
    {
        // Arrange
        var guidString = Guid.Empty.ToString();

        // Act
        var result = TenantId.Create(guidString);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void New_GeneratesUniqueGuid()
    {
        // Act
        var tenant1 = TenantId.New();
        var tenant2 = TenantId.New();

        // Assert
        Assert.NotEqual(tenant1.Value, tenant2.Value);
    }

    [Fact]
    public void ToString_ReturnsGuidString()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var tenantId = TenantId.Create(guid).Value;

        // Act
        var result = tenantId.ToString();

        // Assert
        Assert.Equal(guid.ToString(), result);
    }

    [Fact]
    public void ImplicitOperator_ToGuidConversion()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var tenantId = TenantId.Create(guid).Value;

        // Act
        Guid result = tenantId;

        // Assert
        Assert.Equal(guid, result);
    }

    [Fact]
    public void ImplicitOperator_ToStringConversion()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var tenantId = TenantId.Create(guid).Value;

        // Act
        string result = tenantId;

        // Assert
        Assert.Equal(guid.ToString(), result);
    }

    [Fact]
    public void Equals_SameTenantId_ReturnsTrue()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var tenant1 = TenantId.Create(guid).Value;
        var tenant2 = TenantId.Create(guid).Value;

        // Act & Assert
        Assert.Equal(tenant1, tenant2);
    }

    [Fact]
    public void Equals_DifferentTenantId_ReturnsFalse()
    {
        // Arrange
        var tenant1 = TenantId.New();
        var tenant2 = TenantId.New();

        // Act & Assert
        Assert.NotEqual(tenant1, tenant2);
    }

    [Fact]
    public void GetHashCode_SameTenantId_SameHashCode()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var tenant1 = TenantId.Create(guid).Value;
        var tenant2 = TenantId.Create(guid).Value;

        // Act & Assert
        Assert.Equal(tenant1.GetHashCode(), tenant2.GetHashCode());
    }

    [Fact]
    public void EqualsOperator_SameTenantId_ReturnsTrue()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var tenant1 = TenantId.Create(guid).Value;
        var tenant2 = TenantId.Create(guid).Value;

        // Act & Assert
        Assert.True(tenant1 == tenant2);
    }

    [Fact]
    public void NotEqualsOperator_DifferentTenantId_ReturnsTrue()
    {
        // Arrange
        var tenant1 = TenantId.New();
        var tenant2 = TenantId.New();

        // Act & Assert
        Assert.True(tenant1 != tenant2);
    }

    [Fact]
    public void Create_GuidWithDashes_ReturnsSuccess()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var guidString = guid.ToString("D");

        // Act
        var result = TenantId.Create(guidString);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_GuidWithoutDashes_ReturnsSuccess()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var guidString = guid.ToString("N");

        // Act
        var result = TenantId.Create(guidString);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void New_AlwaysReturnsValidTenantId()
    {
        // Act
        var tenantId = TenantId.New();

        // Assert
        Assert.NotEqual(Guid.Empty, tenantId.Value);
        Assert.True(tenantId.Value != Guid.Empty);
    }

    [Theory]
    [InlineData("550e8400-e29b-41d4-a716-446655440000")]
    [InlineData("{550e8400-e29b-41d4-a716-446655440000}")]
    [InlineData("550e8400e29b41d4a716446655440000")]
    public void Create_DifferentGuidFormats_ReturnsSuccess(string guidString)
    {
        // Act
        var result = TenantId.Create(guidString);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
