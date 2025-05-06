using foodies_api.Models;
using foodies_api.Models.Dtos.Responses;

namespace foodies_api.Interfaces.Services;

public class IAuthService
{
    public ApiResult<RegisterResponse> RegisterResponse { get; set; }
    public ApiResult<LoginResponse> LoginResponse { get; set; }
}