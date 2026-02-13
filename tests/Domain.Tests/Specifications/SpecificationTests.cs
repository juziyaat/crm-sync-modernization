namespace CCA.Sync.Domain.Tests.Specifications;

using CCA.Sync.Domain.Common;
using CCA.Sync.Domain.Specifications;

public class SpecificationTests
{
    private sealed class TestEntity : Entity<int>
    {
        public TestEntity(int id, string name, int age)
        {
            Id = id;
            Name = name;
            Age = age;
        }

        public string Name { get; }

        public int Age { get; }
    }

    private class AdultSpecification : Specification<TestEntity>
    {
        public AdultSpecification()
        {
            Criteria = e => e.Age >= 18;
        }
    }

    private class NameStartsWithASpecification : Specification<TestEntity>
    {
        public NameStartsWithASpecification()
        {
            Criteria = e => e.Name.StartsWith("A");
        }
    }

    [Fact]
    public void IsSatisfiedBy_EntityMatchesCriteria_ReturnsTrue()
    {
        var spec = new AdultSpecification();
        var entity = new TestEntity(1, "John", 25);

        Assert.True(spec.IsSatisfiedBy(entity));
    }

    [Fact]
    public void IsSatisfiedBy_EntityDoesNotMatchCriteria_ReturnsFalse()
    {
        var spec = new AdultSpecification();
        var entity = new TestEntity(1, "John", 15);

        Assert.False(spec.IsSatisfiedBy(entity));
    }

    [Fact]
    public void IsSatisfiedBy_ThrowsIfNull()
    {
        var spec = new AdultSpecification();

        Assert.Throws<ArgumentNullException>(() => spec.IsSatisfiedBy(null!));
    }

    [Fact]
    public void Criteria_CanBeAccessed()
    {
        var spec = new AdultSpecification();

        Assert.NotNull(spec.Criteria);
    }

    [Fact]
    public void Includes_StartsEmpty()
    {
        var spec = new AdultSpecification();

        Assert.Empty(spec.Includes);
    }

    [Fact]
    public void IncludeStrings_StartsEmpty()
    {
        var spec = new AdultSpecification();

        Assert.Empty(spec.IncludeStrings);
    }

    [Fact]
    public void NoCriteria_IsSatisfiedByReturnsTrue()
    {
        var spec = new Specification<TestEntity>();
        var entity = new TestEntity(1, "John", 25);

        Assert.True(spec.IsSatisfiedBy(entity));
    }

    private class Specification<T> : global::CCA.Sync.Domain.Specifications.Specification<T>
    {
    }
}

public class CompositeSpecificationTests
{
    private sealed class TestEntity : Entity<int>
    {
        public TestEntity(int id, string name, int age)
        {
            Id = id;
            Name = name;
            Age = age;
        }

        public string Name { get; }

        public int Age { get; }
    }

    private class AdultSpecification : Specification<TestEntity>
    {
        public AdultSpecification()
        {
            Criteria = e => e.Age >= 18;
        }
    }

    private class NameStartsWithJSpecification : Specification<TestEntity>
    {
        public NameStartsWithJSpecification()
        {
            Criteria = e => e.Name.StartsWith("J");
        }
    }

    [Fact]
    public void AndSpecification_BothSatisfy_ReturnsTrue()
    {
        var spec1 = new AdultSpecification();
        var spec2 = new NameStartsWithJSpecification();
        var andSpec = new AndSpecification<TestEntity>(spec1, spec2);

        var entity = new TestEntity(1, "John", 25);

        Assert.True(andSpec.IsSatisfiedBy(entity));
    }

    [Fact]
    public void AndSpecification_FirstDoesNotSatisfy_ReturnsFalse()
    {
        var spec1 = new AdultSpecification();
        var spec2 = new NameStartsWithJSpecification();
        var andSpec = new AndSpecification<TestEntity>(spec1, spec2);

        var entity = new TestEntity(1, "John", 15);

        Assert.False(andSpec.IsSatisfiedBy(entity));
    }

    [Fact]
    public void AndSpecification_SecondDoesNotSatisfy_ReturnsFalse()
    {
        var spec1 = new AdultSpecification();
        var spec2 = new NameStartsWithJSpecification();
        var andSpec = new AndSpecification<TestEntity>(spec1, spec2);

        var entity = new TestEntity(1, "Alice", 25);

        Assert.False(andSpec.IsSatisfiedBy(entity));
    }

    [Fact]
    public void OrSpecification_FirstSatisfies_ReturnsTrue()
    {
        var spec1 = new AdultSpecification();
        var spec2 = new NameStartsWithJSpecification();
        var orSpec = new OrSpecification<TestEntity>(spec1, spec2);

        var entity = new TestEntity(1, "John", 15);

        Assert.True(orSpec.IsSatisfiedBy(entity));
    }

    [Fact]
    public void OrSpecification_SecondSatisfies_ReturnsTrue()
    {
        var spec1 = new AdultSpecification();
        var spec2 = new NameStartsWithJSpecification();
        var orSpec = new OrSpecification<TestEntity>(spec1, spec2);

        var entity = new TestEntity(1, "John", 15);

        Assert.True(orSpec.IsSatisfiedBy(entity));
    }

    [Fact]
    public void OrSpecification_NeitherSatisfy_ReturnsFalse()
    {
        var spec1 = new AdultSpecification();
        var spec2 = new NameStartsWithJSpecification();
        var orSpec = new OrSpecification<TestEntity>(spec1, spec2);

        var entity = new TestEntity(1, "Alice", 15);

        Assert.False(orSpec.IsSatisfiedBy(entity));
    }

    [Fact]
    public void NotSpecification_SpecificationSatisfies_ReturnsFalse()
    {
        var spec = new AdultSpecification();
        var notSpec = new NotSpecification<TestEntity>(spec);

        var entity = new TestEntity(1, "John", 25);

        Assert.False(notSpec.IsSatisfiedBy(entity));
    }

    [Fact]
    public void NotSpecification_SpecificationDoesNotSatisfy_ReturnsTrue()
    {
        var spec = new AdultSpecification();
        var notSpec = new NotSpecification<TestEntity>(spec);

        var entity = new TestEntity(1, "John", 15);

        Assert.True(notSpec.IsSatisfiedBy(entity));
    }

    [Fact]
    public void ComplexComposition_AndNotOr_WorksTogether()
    {
        var adultSpec = new AdultSpecification();
        var nameSpec = new NameStartsWithJSpecification();
        var andSpec = new AndSpecification<TestEntity>(adultSpec, nameSpec);
        var notSpec = new NotSpecification<TestEntity>(andSpec);

        var entity1 = new TestEntity(1, "John", 25);
        var entity2 = new TestEntity(2, "Alice", 25);

        Assert.False(notSpec.IsSatisfiedBy(entity1));
        Assert.True(notSpec.IsSatisfiedBy(entity2));
    }

    [Fact]
    public void AndSpecification_ThrowsIfNull()
    {
        var spec = new AdultSpecification();

        Assert.Throws<ArgumentNullException>(() => new AndSpecification<TestEntity>(null!, spec));
        Assert.Throws<ArgumentNullException>(() => new AndSpecification<TestEntity>(spec, null!));
    }

    [Fact]
    public void OrSpecification_ThrowsIfNull()
    {
        var spec = new AdultSpecification();

        Assert.Throws<ArgumentNullException>(() => new OrSpecification<TestEntity>(null!, spec));
        Assert.Throws<ArgumentNullException>(() => new OrSpecification<TestEntity>(spec, null!));
    }

    [Fact]
    public void NotSpecification_ThrowsIfNull()
    {
        Assert.Throws<ArgumentNullException>(() => new NotSpecification<TestEntity>(null!));
    }

    private class Specification<T> : global::CCA.Sync.Domain.Specifications.Specification<T>
    {
    }
}
