namespace CCA.Sync.Domain.Tests.Common;

using CCA.Sync.Domain.Common;

public class ErrorTests
{
    [Fact]
    public void Constructor_SetsCodeAndDescription()
    {
        var error = new Error("CODE_001", "Description");

        Assert.Equal("CODE_001", error.Code);
        Assert.Equal("Description", error.Description);
    }

    [Fact]
    public void None_HasEmptyCodeAndDescription()
    {
        var none = Error.None;

        Assert.Equal(string.Empty, none.Code);
        Assert.Equal(string.Empty, none.Description);
    }

    [Fact]
    public void Equals_SameCodeAndDescription_ReturnsTrue()
    {
        var error1 = new Error("CODE", "Description");
        var error2 = new Error("CODE", "Description");

        Assert.Equal(error1, error2);
    }

    [Fact]
    public void Equals_DifferentCode_ReturnsFalse()
    {
        var error1 = new Error("CODE1", "Description");
        var error2 = new Error("CODE2", "Description");

        Assert.NotEqual(error1, error2);
    }

    [Fact]
    public void Equals_DifferentDescription_ReturnsFalse()
    {
        var error1 = new Error("CODE", "Description1");
        var error2 = new Error("CODE", "Description2");

        Assert.NotEqual(error1, error2);
    }

    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        var error = new Error("CODE", "Description");

        Assert.NotEqual(error, null);
    }

    [Fact]
    public void GetHashCode_SameValues_ReturnsSameHashCode()
    {
        var error1 = new Error("CODE", "Description");
        var error2 = new Error("CODE", "Description");

        Assert.Equal(error1.GetHashCode(), error2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DifferentValues_ReturnsDifferentHashCode()
    {
        var error1 = new Error("CODE1", "Description");
        var error2 = new Error("CODE2", "Description");

        Assert.NotEqual(error1.GetHashCode(), error2.GetHashCode());
    }

    [Fact]
    public void EqualityOperator_SameValues_ReturnsTrue()
    {
        var error1 = new Error("CODE", "Description");
        var error2 = new Error("CODE", "Description");

        Assert.True(error1 == error2);
    }

    [Fact]
    public void InequalityOperator_DifferentValues_ReturnsTrue()
    {
        var error1 = new Error("CODE1", "Description");
        var error2 = new Error("CODE2", "Description");

        Assert.True(error1 != error2);
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        var error = new Error("CODE_001", "Something went wrong");

        Assert.Equal("CODE_001: Something went wrong", error.ToString());
    }

    [Fact]
    public void EqualsObject_SameValues_ReturnsTrue()
    {
        var error1 = new Error("CODE", "Description");
        object error2 = new Error("CODE", "Description");

        Assert.True(error1.Equals(error2));
    }
}
