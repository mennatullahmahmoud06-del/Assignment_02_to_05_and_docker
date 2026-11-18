using System.Text.Json.Serialization;

namespace Assignment.Results
{
    public enum ErrorKind { None, Validation, NotFound, Conflict, Forbidden }

    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string Error { get; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ErrorKind Kind { get; }

        public Result(bool isSuccess, string error, ErrorKind kind = ErrorKind.None)
        {
            IsSuccess = isSuccess;
            Error = error;
            Kind = kind;
        }

        public static Result Success() => new Result(true, string.Empty);
        public static Result Failure(string? error, ErrorKind kind = ErrorKind.Validation)
            => new Result(false, error ?? string.Empty, kind);
    }
}
