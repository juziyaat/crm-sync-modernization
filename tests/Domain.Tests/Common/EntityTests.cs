namespace CCA.Sync.Domain.Tests.Common;

using CCA.Sync.Domain.Common;

public class EntityTests
{
    private sealed class TestEntity : Entity<int>
    {
        public TestEntity(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; set; }
    }

    [Fact]
    public void Constructor_SetsId()
    {
        var entity = new TestEntity(1, "Test");
        Assert.Equal(1, entity.Id);
    }

    [Fact]
    public void Equals_SameId_ReturnsTrue()
    {
        var entity1 = new TestEntity(1, "Test1");
        var entity2 = new TestEntity(1, "Test2");

        Assert.Equal(entity1, entity2);
    }

    [Fact]
    public void Equals_DifferentId_ReturnsFalse()
    {
        var entity1 = new TestEntity(1, "Test");
        var entity2 = new TestEntity(2, "Test");

        Assert.NotEqual(entity1, entity2);
    }

    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        var entity = new TestEntity(1, "Test");

        Assert.NotEqual(entity, null);
    }

    [Fact]
    public void GetHashCode_SameId_ReturnsSameHashCode()
    {
        var entity1 = new TestEntity(1, "Test1");
        var entity2 = new TestEntity(1, "Test2");

        Assert.Equal(entity1.GetHashCode(), entity2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DifferentId_ReturnsDifferentHashCode()
    {
        var entity1 = new TestEntity(1, "Test");
        var entity2 = new TestEntity(2, "Test");

        Assert.NotEqual(entity1.GetHashCode(), entity2.GetHashCode());
    }

    [Fact]
    public void EqualityOperator_SameId_ReturnsTrue()
    {
        var entity1 = new TestEntity(1, "Test1");
        var entity2 = new TestEntity(1, "Test2");

        Assert.True(entity1 == entity2);
    }

    [Fact]
    public void EqualityOperator_DifferentId_ReturnsFalse()
    {
        var entity1 = new TestEntity(1, "Test");
        var entity2 = new TestEntity(2, "Test");

        Assert.False(entity1 == entity2);
    }

    [Fact]
    public void InequalityOperator_DifferentId_ReturnsTrue()
    {
        var entity1 = new TestEntity(1, "Test");
        var entity2 = new TestEntity(2, "Test");

        Assert.True(entity1 != entity2);
    }

    [Fact]
    public void InequalityOperator_SameId_ReturnsFalse()
    {
        var entity1 = new TestEntity(1, "Test1");
        var entity2 = new TestEntity(1, "Test2");

        Assert.False(entity1 != entity2);
    }
}
