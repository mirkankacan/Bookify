using System.Diagnostics.CodeAnalysis;

namespace Bookify.Domain.Abstractions
{
    public class Result
    {
        protected internal Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
            {
                throw new InvalidOperationException("Cannot create a sucessful result with an error");
            }
            if (!isSuccess && error == Error.None)
            {
                throw new InvalidOperationException("Cannot create a failed result without an error");
            }
            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; private set; }
        public Error Error { get; private set; }
        public bool IsFailure => !IsSuccess;

        public static Result Success() => new(true, Error.None);

        public static Result Failure(Error error) => new(false, error);

        public static Result<TValue> Success<TValue>(TValue value)
        {
            return new Result<TValue>(value, true, Error.None);
        }

        public static Result<TValue> Failure<TValue>(Error error)
        {
            return new Result<TValue>(default, false, error);
        }

        public static Result<TValue> Create<TValue>(TValue? value)
        {
            return value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
        }
    }

    public sealed class Result<TValue> : Result
    {
        private readonly TValue? _value;

        public Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            _value = value;
        }

        [NotNull]
        public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException("The value of failure result can not be accessed");

        public static implicit operator Result<TValue>(TValue value) => Create(value);
    }
}