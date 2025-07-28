using foodies_api.Models;
using foodies_api.Models.Dtos.Requests;
using foodies_api.Models.Dtos.Responses;

namespace foodies_api.Interfaces.Services;

public interface IAuthService
{
    public Task<ApiResult<RegisterResponse>> Register(RegisterRequest request);
    public Task<ApiResult<LoginResponse>> Login(LoginRequest request);
    public Task<ApiResult<LogoutResponse>> Logout();
}