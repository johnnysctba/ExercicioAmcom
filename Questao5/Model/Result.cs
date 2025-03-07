using Questao5.Domain.Responses;

namespace Questao5.Model
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        public string Error { get; }
        public string ErrorType { get; }

        private Result(bool isSuccess, T value, string error, string errorType)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
            ErrorType = errorType;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(true, value, null, null);
        }

        public static Result<T> Fail(string error, string errorType)
        {
            return new Result<T>(false, default, error, errorType);
        }

        public ErrorResponse GetError()
        {
            if (IsSuccess)
            {
                throw new InvalidOperationException("Cannot get error from a successful result.");
            }

            return new ErrorResponse
            {
                ErrorType = ErrorType,
                Message = Error
            };
        }
    }
}
