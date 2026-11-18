namespace Assignment.Results
{
    public class GenericResult<T> : Result
    {
        public T? Value { get; set; }

        public GenericResult(bool isSuccess, T? value, string error, ErrorKind kind = ErrorKind.None)
            : base(isSuccess, error, kind)
        {
            Value = value;
        }

        public static GenericResult<T> Success(T value)
            => new GenericResult<T>(true, value, string.Empty);

        public static new GenericResult<T> Failure(string? error = null, ErrorKind kind = ErrorKind.Validation)
            => new GenericResult<T>(false, default, error ?? string.Empty, kind);
    }
}
