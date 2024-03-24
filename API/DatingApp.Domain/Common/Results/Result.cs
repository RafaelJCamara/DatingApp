using DatingApp.Domain.Common.Errors;

namespace DatingApp.Domain.Common.Response
{
    public class Result
    {
        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
                throw new ArgumentException("Invalid error", nameof(error));

            IsSuccess = isSuccess;
            Error = error;
        }


        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public Error Error { get; }

        public static Result Success() => new(true, Error.None);

        public static Result Failure(Error error) => new(false, error);
    }

    public class Result<T> : Result
    {
        public Result(bool isSuccess, Error error, T? value = default) : base(isSuccess, error)
        {
            Value = value;
        }

        public T? Value { get; }

        public static Result<T> Success(T value) => new(true, Error.None, value);

        public static new Result<T?> Failure(Error error) => new(false, error);
    }
}
