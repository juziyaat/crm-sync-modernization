namespace CCA.Sync.Domain.Tests.Common;

using CCA.Sync.Domain.Common;

public class ValueObjectTests
{
    private sealed class TestValueObject : ValueObject
    {
        public TestValueObject(string value1, int value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public string Value1 { get; }

        public int Value2 { get; }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value1;
            yield return Value2;
        }
    }

    [Fact]
    public void Equals_SameValues_ReturnsTrue()
    {
        var vo1 = new TestValueObject("test", 42);
        var vo2 = new TestValueObject("test", 42);

        Assert.Equal(vo1, vo2);
    }

    [Fact]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        var vo1 = new TestValueObject("test1", 42);
        var vo2 = new TestValueObject("test2", 42);

        Assert.NotEqual(vo1, vo2);
    }

    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        var vo = new TestValueObject("test", 42);

        Assert.NotEqual(vo, null);
    }

    [Fact]
    public void Equals_DifferentSecondValue_ReturnsFalse()
    {
        var vo1 = new TestValueObject("test", 42);
        var vo2 = new TestValueObject("test", 43);

        Assert.NotEqual(vo1, vo2);
    }

    [Fact]
    public void GetHashCode_SameValues_ReturnsSameHashCode()
    {
        var vo1 = new TestValueObject("test", 42);
        var vo2 = new TestValueObject("test", 42);

        Assert.Equal(vo1.GetHashCode(), vo2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DifferentValues_ReturnsDifferentHashCode()
    {
        var vo1 = new TestValueObject("test1", 42);
        var vo2 = new TestValueObject("test2", 42);

        Assert.NotEqual(vo1.GetHashCode(), vo2.GetHashCode());
    }

    [Fact]
    public void EqualityOperator_SameValues_ReturnsTrue()
    {
        var vo1 = new TestValueObject("test", 42);
        var vo2 = new TestValueObject("test", 42);

        Assert.True(vo1 == vo2);
    }

    [Fact]
    public void EqualityOperator_DifferentValues_ReturnsFalse()
    {
        var vo1 = new TestValueObject("test1", 42);
        var vo2 = new TestValueObject("test2", 42);

        Assert.False(vo1 == vo2);
    }

    [Fact]
    public void InequalityOperator_DifferentValues_ReturnsTrue()
    {
        var vo1 = new TestValueObject("test1", 42);
        var vo2 = new TestValueObject("test2", 42);

        Assert.True(vo1 != vo2);
    }

    [Fact]
    public void InequalityOperator_SameValues_ReturnsFalse()
    {
        var vo1 = new TestValueObject("test", 42);
        var vo2 = new TestValueObject("test", 42);

        Assert.False(vo1 != vo2);
    }
}
