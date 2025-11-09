namespace Web.API.Shared
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Success constructors
        public static ApiResponse<T> Ok(T data, string message = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message,
                Errors = new()
            };
        }
        public static ApiResponse<T> Ok(string message)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = default,
                Message = message,
                Errors = new()
            };
        }

        // Failure constructors
        public static ApiResponse<T> Fail(string error, string message = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Data = default,
                Message = message,
                Errors = new List<string> { error }
            };
        }

        public static ApiResponse<T> Fail(List<string> errors, string message = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Data = default,
                Message = message,
                Errors = errors
            };
        }
       
    }

    
    
}