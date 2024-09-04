using System.Net;
using foodies_api.Data;
// using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace foodies_api.Models;

public class ApiResult<T>
{
    public ApiResult()
    {
        ErrorMessages = new List<string>();
    }
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public List<string>? ErrorMessages { get; set; }
    public Exception Exception { get; set; }

    public static ApiResult<T> Fail(string message, HttpStatusCode statusCode, Exception? exception)
        {
            return new ApiResult<T>
            {
                IsSuccess = false,
                Data = default,
                StatusCode = statusCode,
                ErrorMessages = new() {message},
                Exception = exception,
            };
        }

        public static ApiResult<T> Fail(string message, HttpStatusCode statusCode)
        {
            return new ApiResult<T>
            {
                IsSuccess = false,
                Data = default,
                StatusCode = statusCode,
                ErrorMessages = new() {message},
            };
        }

        public static ApiResult<T> Pass(T data)
        {
            return new ApiResult<T>
            {
                IsSuccess = true,
                Data = data,
                StatusCode = HttpStatusCode.OK,
                ErrorMessages = null,
                Exception = default,
            };
        }
}

public class ApiResult
{
    public ApiResult()
    {
        ErrorMessages = new List<string>();
    }
    public bool IsSuccess { get; set; }
    public object Data { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public List<string> ErrorMessages { get; set; }
    public string Exception { get; set; }
}