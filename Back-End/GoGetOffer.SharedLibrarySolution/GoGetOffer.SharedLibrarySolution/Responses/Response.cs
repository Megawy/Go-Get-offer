namespace GoGetOffer.SharedLibrarySolution.Responses
{
    public class Response<T>
    {
        public bool Status { get; init; }
        public string? Message { get; init; }
        public T? Data { get; init; }
        public object? Errors { get; set; }

        // Success cases
        public static Response<T> Success()
            => new() { Status = true };

        public static Response<T> Success(string message = "")
            => new() { Status = true, Message = message };

        public static Response<T> Success(T? data)
            => new() { Status = true, Data = data, Message = string.Empty };

        public static Response<T> Success(T? data, string message)
            => new() { Status = true, Data = data, Message = message };

        public static Response<IEnumerable<T>> Success(IEnumerable<T>? data, string message)
            => new() { Status = true, Data = data, Message = message };

        // Failure cases
        public static Response<T> Failure(string message)
            => new() { Status = false, Message = message };

        public static Response<T> Failure(T? data, string message)
            => new() { Status = false, Data = data, Message = message };

        public static Response<T> Failure(object? errors)
            => new() { Status = false, Errors = errors };
        public static Response<T> Failure(object? errors, string message)
            => new() { Status = false, Errors = errors, Message = message };

        public static Response<IEnumerable<T>> Failure(IEnumerable<T>? data, string message)
            => new() { Status = false, Data = data, Message = message };
        public static Response<IEnumerable<T>> Failure(IEnumerable<object>? errors, string message)
            => new() { Status = false, Errors = errors, Message = message };
    }
}
