using System.Net;
using AutoMapper;
using foodies_api.Auth;
using foodies_api.Interfaces.Repositories;
using foodies_api.Interfaces.Services;
using foodies_api.Models;
using foodies_api.Models.Dtos.Requests;
using foodies_api.Models.Dtos.Responses;
using foodies_api.Models.Entities;

public class AuthService : IAuthService
{
    public IHttpContextAccessor _httpContext;
    public ILogger<AuthService> _logger;
    public IMapper _mapper;
    public IAuthRepository _repository;
    public IConfiguration _config;
    public AuthService(
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuthService> logger,
        IMapper mapper,
        IAuthRepository repository,
        IConfiguration config)
    {
        _httpContext = httpContextAccessor;
        _logger = logger;
        _mapper = mapper;
        _repository = repository;
        _config = config;

    }
    public async Task<ApiResult<RegisterResponse>> Register(RegisterRequest request)
    {
        var user = _mapper.Map<User>(request);
        var result = await _repository.Register(user);

        if (!result.Success)
        {
            _logger.LogError("Failed to register user");
            return new ApiResult<RegisterResponse>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = result.Errors,
                Message = result.Message
            };
        }
        var AuthConfig = new Authentication(_config);
        var token = AuthConfig.CreateAccessToken(result.Data);
        var registerResponse = _mapper.Map<RegisterResponse>(result.Data);
        registerResponse.Token = token;

        _logger.LogInformation("Successfully registered a user");

        return new ApiResult<RegisterResponse>
        {
            Data = registerResponse,
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = result.Message
        };
    }

    public async Task<ApiResult<LoginResponse>> Login(LoginRequest request)
    {
        var result = await _repository.Login(request);

        if (!result.Success)
        {
            _logger.LogError("Failed to login user");
            return new ApiResult<LoginResponse>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = result.Errors,
                Message = result.Message
            };
        }

        var AuthConfig = new Authentication(_config);
        var token = AuthConfig.CreateAccessToken(result.Data);
        var loginResponse = _mapper.Map<LoginResponse>(result.Data);
        loginResponse.Token = token;

        _logger.LogInformation("Successfully logged in");
        return new ApiResult<LoginResponse>
        {
            Data = loginResponse,
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = result.Message
        };
    }
}