namespace CCA.Sync.Domain.Tests.Common;

using CCA.Sync.Domain.Common;

public class ResultTests
{
    [Fact]
    public void Success_CreatesSuccessfulResult()
    {
        var result = Result<int>.Success(42);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }

    [Fact]
    public void Success_StoresValue()
    {
        var result = Result<string>.Success("test");

        Assert.Equal("test", result.Value);
    }

    [Fact]
    public void Success_ThrowsIfNull()
    {
        Assert.Throws<ArgumentNullException>(() => Result<string>.Success(null!));
    }

    [Fact]
    public void Failure_CreatesFailedResult()
    {
        var error = new Error("CODE", "Description");
        var result = Result<int>.Failure(error);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Failure_StoresError()
    {
        var error = new Error("CODE", "Description");
        var result = Result<int>.Failure(error);

        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Failure_ThrowsIfNull()
    {
        Assert.Throws<ArgumentNullException>(() => Result<int>.Failure(null!));
    }

    [Fact]
    public void Value_ThrowsIfFailure()
    {
        var error = new Error("CODE", "Description");
        var result = Result<int>.Failure(error);

        Assert.Throws<InvalidOperationException>(() => result.Value);
    }

    [Fact]
    public void Error_ThrowsIfSuccess()
    {
        var result = Result<int>.Success(42);

        Assert.Throws<InvalidOperationException>(() => result.Error);
    }

    [Fact]
    public void Match_WithSuccess_ExecutesOnSuccess()
    {
        var result = Result<int>.Success(42);
        var executed = false;

        result.Match(
            value =>
            {
                executed = true;
                Assert.Equal(42, value);
            },
            _ => Assert.True(false, "Should not execute failure"));

        Assert.True(executed);
    }

    [Fact]
    public void Match_WithFailure_ExecutesOnFailure()
    {
        var error = new Error("CODE", "Description");
        var result = Result<int>.Failure(error);
        var executed = false;

        result.Match(
            _ => Assert.True(false, "Should not execute success"),
            e =>
            {
                executed = true;
                Assert.Equal(error, e);
            });

        Assert.True(executed);
    }

    [Fact]
    public void Match_WithFunction_ReturnsSuccessValue()
    {
        var result = Result<int>.Success(42);

        var output = result.Match(
            value => value * 2,
            _ => 0);

        Assert.Equal(84, output);
    }

    [Fact]
    public void Match_WithFunction_ReturnsFailureResult()
    {
        var error = new Error("CODE", "Description");
        var result = Result<int>.Failure(error);

        var output = result.Match(
            _ => 0,
            _ => -1);

        Assert.Equal(-1, output);
    }

    [Fact]
    public void Map_WithSuccess_ProjectsValue()
    {
        var result = Result<int>.Success(42);

        var mapped = result.Map(v => v * 2);

        Assert.True(mapped.IsSuccess);
        Assert.Equal(84, mapped.Value);
    }

    [Fact]
    public void Map_WithFailure_PreservesError()
    {
        var error = new Error("CODE", "Description");
        var result = Result<int>.Failure(error);

        var mapped = result.Map(v => v * 2);

        Assert.True(mapped.IsFailure);
        Assert.Equal(error, mapped.Error);
    }

    [Fact]
    public void Bind_WithSuccess_ReturnsNewResult()
    {
        var result = Result<int>.Success(42);

        var bound = result.Bind(v => Result<string>.Success(v.ToString()));

        Assert.True(bound.IsSuccess);
        Assert.Equal("42", bound.Value);
    }

    [Fact]
    public void Bind_WithFailure_PreservesError()
    {
        var error = new Error("CODE", "Description");
        var result = Result<int>.Failure(error);

        var bound = result.Bind(v => Result<string>.Success(v.ToString()));

        Assert.True(bound.IsFailure);
        Assert.Equal(error, bound.Error);
    }

    [Fact]
    public void Equals_SameSuccess_ReturnsTrue()
    {
        var result1 = Result<int>.Success(42);
        var result2 = Result<int>.Success(42);

        Assert.Equal(result1, result2);
    }

    [Fact]
    public void Equals_DifferentSuccess_ReturnsFalse()
    {
        var result1 = Result<int>.Success(42);
        var result2 = Result<int>.Success(43);

        Assert.NotEqual(result1, result2);
    }

    [Fact]
    public void Equals_SameFailure_ReturnsTrue()
    {
        var error = new Error("CODE", "Description");
        var result1 = Result<int>.Failure(error);
        var result2 = Result<int>.Failure(error);

        Assert.Equal(result1, result2);
    }

    [Fact]
    public void ToString_WithSuccess_ReturnsFormattedString()
    {
        var result = Result<int>.Success(42);

        Assert.Equal("Success(42)", result.ToString());
    }

    [Fact]
    public void ToString_WithFailure_ReturnsFormattedString()
    {
        var error = new Error("CODE", "Description");
        var result = Result<int>.Failure(error);

        Assert.Equal($"Failure({error})", result.ToString());
    }
}

public class ResultNonGenericTests
{
    [Fact]
    public void Success_CreatesSuccessfulResult()
    {
        var result = Result.Success();

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }

    [Fact]
    public void Failure_CreatesFailedResult()
    {
        var error = new Error("CODE", "Description");
        var result = Result.Failure(error);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Failure_ThrowsIfNull()
    {
        Assert.Throws<ArgumentNullException>(() => Result.Failure(null!));
    }

    [Fact]
    public void Error_ThrowsIfSuccess()
    {
        var result = Result.Success();

        Assert.Throws<InvalidOperationException>(() => result.Error);
    }

    [Fact]
    public void Match_WithSuccess_ExecutesOnSuccess()
    {
        var result = Result.Success();
        var executed = false;

        result.Match(
            () => executed = true,
            _ => Assert.True(false));

        Assert.True(executed);
    }

    [Fact]
    public void Match_WithFunction_ReturnsSuccessValue()
    {
        var result = Result.Success();

        var output = result.Match(
            () => 42,
            _ => 0);

        Assert.Equal(42, output);
    }

    [Fact]
    public void Equals_SameSuccess_ReturnsTrue()
    {
        var result1 = Result.Success();
        var result2 = Result.Success();

        Assert.Equal(result1, result2);
    }

    [Fact]
    public void ToString_WithSuccess_ReturnsFormattedString()
    {
        var result = Result.Success();

        Assert.Equal("Success()", result.ToString());
    }
}
