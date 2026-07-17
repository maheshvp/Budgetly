namespace Budgetly.Application.Common.Results;

public sealed class Result<T> : Result
{
    private readonly T? _value;

    private Result(bool isSuccess, Error error, T? value) : base(isSuccess, error)
    {
        _value = value;
    }

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access the value of a failed result.");

    public static Result<T> Success(T value) => new(true, Error.None, value);
    public new static Result<T> Failure(Error error) => new(false, error, default);
}
