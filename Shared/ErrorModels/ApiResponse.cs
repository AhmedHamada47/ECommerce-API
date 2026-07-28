using System.Text.Json.Serialization;

namespace Shared.ErrorModels
{
    public class ApiResponse<T>
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<ApiError>? Errors { get; set; }

        public static ApiResponse<T> Success(T data, string? message = null) => new()
        {
            Data = data,
            Message = message
        };

        public static ApiResponse<T> Failure(string message, IEnumerable<ApiError>? errors = null) => new()
        {
            Message = message,
            Errors = errors
        };
    }

    public class ApiError
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Field { get; set; }
    }
}