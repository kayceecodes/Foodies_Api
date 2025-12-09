using System;

namespace foodies_api.Models.Dtos.Responses;

public class RepositoryResponse<T>
{
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new();

        public static RepositoryResponse<T> SetSuccessfulData(T data)
        {
            return new RepositoryResponse<T>()
            {
                Success = true,
                Data = data,
                Message = null
            };
        }

        public static RepositoryResponse<T> SetUnsucessfulData(Exception exception, T data = default)
        {
            return new RepositoryResponse<T>()
            {
                Success = false,
                Data = data,
                Errors = [] 
            };
        }

}
