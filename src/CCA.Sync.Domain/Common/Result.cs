namespace CCA.Sync.Domain.Common;

/// <summary>
/// Represents a result that can be either a success with a value or a failure with an error.
/// </summary>
/// <typeparam name="TValue">The type of the success value</typeparam>
public sealed class Result<TValue> : IEquatable<Result<TValue>>
{
    private readonly TValue? _value;
    private readonly Error? _error;

    private Result(TValue value)
    {
        _value = value;
        _error = null;
        IsSuccess = true;
    }

    private Result(Error error)
    {
        _value = default;
        _error = error;
        IsSuccess = false;
    }

    /// <summary>
    /// Gets a value indicating whether the result represents a successful operation.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the result represents a failed operation.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the success value. Throws if the result is a failure.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when attempting to get value from a failed result</exception>
    public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException("Cannot access value of a failed result.");

    /// <summary>
    /// Gets the error. Throws if the result is a success.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when attempting to get error from a successful result</exception>
    public Error Error => IsFailure ? _error! : throw new InvalidOperationException("Cannot access error of a successful result.");

    /// <summary>
    /// Creates a successful result with the given value.
    /// </summary>
    /// <param name="value">The success value</param>
    /// <returns>A successful result</returns>
    public static Result<TValue> Success(TValue value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new Result<TValue>(value);
    }

    /// <summary>
    /// Creates a failed result with the given error.
    /// </summary>
    /// <param name="error">The error</param>
    /// <returns>A failed result</returns>
    public static Result<TValue> Failure(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return new Result<TValue>(error);
    }

    /// <summary>
    /// Executes a function based on whether the result is a success or failure.
    /// </summary>
    /// <typeparam name="TResult">The result type</typeparam>
    /// <param name="onSuccess">The function to execute if successful</param>
    /// <param name="onFailure">The function to execute if failed</param>
    /// <returns>The result of the executed function</returns>
    public TResult Match<TResult>(
        Func<TValue, TResult> onSuccess,
        Func<Error, TResult> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        return IsSuccess ? onSuccess(Value) : onFailure(Error);
    }

    /// <summary>
    /// Executes an action based on whether the result is a success or failure.
    /// </summary>
    /// <param name="onSuccess">The action to execute if successful</param>
    /// <param name="onFailure">The action to execute if failed</param>
    public void Match(
        Action<TValue> onSuccess,
        Action<Error> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        if (IsSuccess)
        {
            onSuccess(Value);
        }
        else
        {
            onFailure(Error);
        }
    }

    /// <summary>
    /// Projects the success value to a new type.
    /// </summary>
    /// <typeparam name="TNewValue">The new value type</typeparam>
    /// <param name="selector">The projection function</param>
    /// <returns>A new result with the projected value or the original error</returns>
    public Result<TNewValue> Map<TNewValue>(Func<TValue, TNewValue> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        return IsSuccess
            ? Result<TNewValue>.Success(selector(Value))
            : Result<TNewValue>.Failure(Error);
    }

    /// <summary>
    /// Projects the success value to a new result type.
    /// </summary>
    /// <typeparam name="TNewValue">The new value type</typeparam>
    /// <param name="selector">The projection function</param>
    /// <returns>A new result with the projected result or the original error</returns>
    public Result<TNewValue> Bind<TNewValue>(Func<TValue, Result<TNewValue>> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        return IsSuccess
            ? selector(Value)
            : Result<TNewValue>.Failure(Error);
    }

    /// <summary>
    /// Determines whether two results are equal.
    /// </summary>
    public bool Equals(Result<TValue>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (IsSuccess != other.IsSuccess)
        {
            return false;
        }

        return IsSuccess
            ? EqualityComparer<TValue>.Default.Equals(Value, other.Value)
            : Error == other.Error;
    }

    /// <summary>
    /// Determines whether two results are equal.
    /// </summary>
    public override bool Equals(object? obj)
    {
        return obj is Result<TValue> result && Equals(result);
    }

    /// <summary>
    /// Gets the hash code for this result.
    /// </summary>
    public override int GetHashCode()
    {
        return IsSuccess
            ? HashCode.Combine(IsSuccess, Value)
            : HashCode.Combine(IsSuccess, Error);
    }

    /// <summary>
    /// Gets the string representation of this result.
    /// </summary>
    public override string ToString()
    {
        return IsSuccess
            ? $"Success({Value})"
            : $"Failure({Error})";
    }

    /// <summary>
    /// Determines whether two results are equal.
    /// </summary>
    public static bool operator ==(Result<TValue>? left, Result<TValue>? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two results are not equal.
    /// </summary>
    public static bool operator !=(Result<TValue>? left, Result<TValue>? right)
    {
        return !Equals(left, right);
    }
}

/// <summary>
/// Represents a result that can be either a success or a failure with an error.
/// </summary>
public sealed class Result : IEquatable<Result>
{
    private readonly Error? _error;

    private Result()
    {
        _error = null;
        IsSuccess = true;
    }

    private Result(Error error)
    {
        _error = error;
        IsSuccess = false;
    }

    /// <summary>
    /// Gets a value indicating whether the result represents a successful operation.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the result represents a failed operation.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the error. Throws if the result is a success.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when attempting to get error from a successful result</exception>
    public Error Error => IsFailure ? _error! : throw new InvalidOperationException("Cannot access error of a successful result.");

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <returns>A successful result</returns>
    public static Result Success()
    {
        return new Result();
    }

    /// <summary>
    /// Creates a failed result with the given error.
    /// </summary>
    /// <param name="error">The error</param>
    /// <returns>A failed result</returns>
    public static Result Failure(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return new Result(error);
    }

    /// <summary>
    /// Executes a function based on whether the result is a success or failure.
    /// </summary>
    /// <typeparam name="TResult">The result type</typeparam>
    /// <param name="onSuccess">The function to execute if successful</param>
    /// <param name="onFailure">The function to execute if failed</param>
    /// <returns>The result of the executed function</returns>
    public TResult Match<TResult>(
        Func<TResult> onSuccess,
        Func<Error, TResult> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        return IsSuccess ? onSuccess() : onFailure(Error);
    }

    /// <summary>
    /// Executes an action based on whether the result is a success or failure.
    /// </summary>
    /// <param name="onSuccess">The action to execute if successful</param>
    /// <param name="onFailure">The action to execute if failed</param>
    public void Match(
        Action onSuccess,
        Action<Error> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        if (IsSuccess)
        {
            onSuccess();
        }
        else
        {
            onFailure(Error);
        }
    }

    /// <summary>
    /// Determines whether two results are equal.
    /// </summary>
    public bool Equals(Result? other)
    {
        if (other is null)
        {
            return false;
        }

        if (IsSuccess != other.IsSuccess)
        {
            return false;
        }

        return IsSuccess || Error == other.Error;
    }

    /// <summary>
    /// Determines whether two results are equal.
    /// </summary>
    public override bool Equals(object? obj)
    {
        return obj is Result result && Equals(result);
    }

    /// <summary>
    /// Gets the hash code for this result.
    /// </summary>
    public override int GetHashCode()
    {
        return IsSuccess ? 1 : Error.GetHashCode();
    }

    /// <summary>
    /// Gets the string representation of this result.
    /// </summary>
    public override string ToString()
    {
        return IsSuccess ? "Success()" : $"Failure({Error})";
    }

    /// <summary>
    /// Determines whether two results are equal.
    /// </summary>
    public static bool operator ==(Result? left, Result? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two results are not equal.
    /// </summary>
    public static bool operator !=(Result? left, Result? right)
    {
        return !Equals(left, right);
    }
}
