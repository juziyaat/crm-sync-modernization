using CCA.Sync.Domain.Common;
using CCA.Sync.Domain.ValueObjects;

namespace CCA.Sync.Domain.Tests.ValueObjects;

/// <summary>
/// Tests for the CustomerName value object.
/// </summary>
public class CustomerNameTests
{
    [Fact]
    public void Create_ValidFirstAndLastName_ReturnsSuccess()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";

        // Act
        var result = CustomerName.Create(firstName, lastName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("John", result.Value.FirstName);
        Assert.Equal("Doe", result.Value.LastName);
        Assert.Null(result.Value.MiddleName);
    }

    [Fact]
    public void Create_WithMiddleName_ReturnsSuccess()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var middleName = "Paul";

        // Act
        var result = CustomerName.Create(firstName, lastName, middleName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("John", result.Value.FirstName);
        Assert.Equal("Doe", result.Value.LastName);
        Assert.Equal("Paul", result.Value.MiddleName);
    }

    [Fact]
    public void Create_EmptyFirstName_ReturnsFailure()
    {
        // Arrange
        var firstName = "";
        var lastName = "Doe";

        // Act
        var result = CustomerName.Create(firstName, lastName);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("CustomerName.FirstNameEmpty", result.Error.Code);
    }

    [Fact]
    public void Create_EmptyLastName_ReturnsFailure()
    {
        // Arrange
        var firstName = "John";
        var lastName = "";

        // Act
        var result = CustomerName.Create(firstName, lastName);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("CustomerName.LastNameEmpty", result.Error.Code);
    }

    [Fact]
    public void Create_FirstNameTooLong_ReturnsFailure()
    {
        // Arrange
        var firstName = new string('a', 101);
        var lastName = "Doe";

        // Act
        var result = CustomerName.Create(firstName, lastName);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("CustomerName.FirstNameTooLong", result.Error.Code);
    }

    [Fact]
    public void Create_LastNameTooLong_ReturnsFailure()
    {
        // Arrange
        var firstName = "John";
        var lastName = new string('a', 101);

        // Act
        var result = CustomerName.Create(firstName, lastName);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("CustomerName.LastNameTooLong", result.Error.Code);
    }

    [Fact]
    public void Create_MiddleNameTooLong_ReturnsFailure()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var middleName = new string('a', 101);

        // Act
        var result = CustomerName.Create(firstName, lastName, middleName);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("CustomerName.MiddleNameTooLong", result.Error.Code);
    }

    [Fact]
    public void Create_WhitespaceFirstName_ReturnsFailure()
    {
        // Arrange
        var firstName = "   ";
        var lastName = "Doe";

        // Act
        var result = CustomerName.Create(firstName, lastName);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_TrimsWhitespace()
    {
        // Arrange
        var firstName = "  John  ";
        var lastName = "  Doe  ";

        // Act
        var result = CustomerName.Create(firstName, lastName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("John", result.Value.FirstName);
        Assert.Equal("Doe", result.Value.LastName);
    }

    [Fact]
    public void FullName_WithoutMiddleName_ReturnsFirstAndLastOnly()
    {
        // Arrange
        var name = CustomerName.Create("John", "Doe").Value;

        // Act
        var fullName = name.FullName;

        // Assert
        Assert.Equal("John Doe", fullName);
    }

    [Fact]
    public void FullName_WithMiddleName_IncludesMiddleName()
    {
        // Arrange
        var name = CustomerName.Create("John", "Doe", "Paul").Value;

        // Act
        var fullName = name.FullName;

        // Assert
        Assert.Equal("John Paul Doe", fullName);
    }

    [Fact]
    public void FullName_WithEmptyMiddleName_ExcludesMiddleName()
    {
        // Arrange
        var name = CustomerName.Create("John", "Doe", "").Value;

        // Act
        var fullName = name.FullName;

        // Assert
        Assert.Equal("John Doe", fullName);
    }

    [Fact]
    public void ToString_ReturnsFullName()
    {
        // Arrange
        var name = CustomerName.Create("John", "Doe").Value;

        // Act
        var result = name.ToString();

        // Assert
        Assert.Equal("John Doe", result);
    }

    [Fact]
    public void ImplicitOperator_ToStringConversion()
    {
        // Arrange
        var name = CustomerName.Create("John", "Doe").Value;

        // Act
        string result = name;

        // Assert
        Assert.Equal("John Doe", result);
    }

    [Fact]
    public void Equals_SameName_ReturnsTrue()
    {
        // Arrange
        var name1 = CustomerName.Create("John", "Doe").Value;
        var name2 = CustomerName.Create("John", "Doe").Value;

        // Act & Assert
        Assert.Equal(name1, name2);
    }

    [Fact]
    public void Equals_WithMiddleName_ReturnsTrue()
    {
        // Arrange
        var name1 = CustomerName.Create("John", "Doe", "Paul").Value;
        var name2 = CustomerName.Create("John", "Doe", "Paul").Value;

        // Act & Assert
        Assert.Equal(name1, name2);
    }

    [Fact]
    public void Equals_CaseInsensitive_ReturnsTrue()
    {
        // Arrange
        var name1 = CustomerName.Create("John", "Doe").Value;
        var name2 = CustomerName.Create("JOHN", "DOE").Value;

        // Act & Assert
        Assert.Equal(name1, name2);
    }

    [Fact]
    public void Equals_DifferentFirstName_ReturnsFalse()
    {
        // Arrange
        var name1 = CustomerName.Create("John", "Doe").Value;
        var name2 = CustomerName.Create("Jane", "Doe").Value;

        // Act & Assert
        Assert.NotEqual(name1, name2);
    }

    [Fact]
    public void Equals_DifferentLastName_ReturnsFalse()
    {
        // Arrange
        var name1 = CustomerName.Create("John", "Doe").Value;
        var name2 = CustomerName.Create("John", "Smith").Value;

        // Act & Assert
        Assert.NotEqual(name1, name2);
    }

    [Fact]
    public void Equals_DifferentMiddleName_ReturnsFalse()
    {
        // Arrange
        var name1 = CustomerName.Create("John", "Doe", "Paul").Value;
        var name2 = CustomerName.Create("John", "Doe", "Michael").Value;

        // Act & Assert
        Assert.NotEqual(name1, name2);
    }

    [Fact]
    public void Equals_OneWithMiddleOneWithout_ReturnsFalse()
    {
        // Arrange
        var name1 = CustomerName.Create("John", "Doe", "Paul").Value;
        var name2 = CustomerName.Create("John", "Doe").Value;

        // Act & Assert
        Assert.NotEqual(name1, name2);
    }

    [Fact]
    public void GetHashCode_SameName_SameHashCode()
    {
        // Arrange
        var name1 = CustomerName.Create("John", "Doe").Value;
        var name2 = CustomerName.Create("John", "Doe").Value;

        // Act & Assert
        Assert.Equal(name1.GetHashCode(), name2.GetHashCode());
    }

    [Fact]
    public void EqualsOperator_SameName_ReturnsTrue()
    {
        // Arrange
        var name1 = CustomerName.Create("John", "Doe").Value;
        var name2 = CustomerName.Create("John", "Doe").Value;

        // Act & Assert
        Assert.True(name1 == name2);
    }

    [Fact]
    public void NotEqualsOperator_DifferentName_ReturnsTrue()
    {
        // Arrange
        var name1 = CustomerName.Create("John", "Doe").Value;
        var name2 = CustomerName.Create("Jane", "Smith").Value;

        // Act & Assert
        Assert.True(name1 != name2);
    }

    [Theory]
    [InlineData("John", "Doe")]
    [InlineData("Mary", "Johnson")]
    [InlineData("Robert", "Williams")]
    public void Create_CommonNames_ReturnsSuccess(string firstName, string lastName)
    {
        // Act
        var result = CustomerName.Create(firstName, lastName);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_MaxLengthNames_ReturnsSuccess()
    {
        // Arrange
        var firstName = new string('a', 100);
        var lastName = new string('b', 100);

        // Act
        var result = CustomerName.Create(firstName, lastName);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
