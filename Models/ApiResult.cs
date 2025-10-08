using System.Diagnostics.CodeAnalysis;
using System.Net;
using foodies_api.Data;
// using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace foodies_api.Models;

public class ApiResult<T>
{
    public ApiResult()
    {
    }
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; } = new();
    public Exception? Exception { get; set; }

    public static ApiResult<T> Fail(List<string> errors, HttpStatusCode statusCode, Exception? exception = null)
        {
            return new ApiResult<T>
            {
                IsSuccess = false,
                Data = default,
                StatusCode = statusCode,
                Errors = errors,
                Exception = exception,
            };
        }

        public static ApiResult<T> Pass(T data)
        {
            return new ApiResult<T>
            {
                IsSuccess = true,
                Data = data,
                StatusCode = HttpStatusCode.OK,
                Errors = null,
                Exception = default,
            };
        }
}

// public class ApiResult
// {
//     public ApiResult()
//     {
//         ErrorMessages = new List<string>();
//     }
//     public bool IsSuccess { get; set; }
//     public object Data { get; set; }
//     public HttpStatusCode StatusCode { get; set; }
//     public string Message { get; set;}
//     public List<string> ErrorMessages { get; set; }
//     public string Exception { get; set; }
// }