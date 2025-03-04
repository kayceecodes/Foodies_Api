using System;
using foodies_api.Models;
using foodies_api.Models.Dtos;
using foodies_api.Models.Dtos.Requests;
using foodies_api.Models.Dtos.Responses;
using foodies_api.Models.Entities;

namespace foodies_api.Interfaces.Services;

public interface IAuthService
{
    public Task<ApiResult<RegistrationResponse>> Register(RegistrationRequest request);
    public Task<ApiResult<LoginResponse>> Login(LoginRequest request);
}
