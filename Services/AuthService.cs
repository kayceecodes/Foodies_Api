using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using foodies_api.Models;
using foodies_api.Repositories;
using foodies_api.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using foodies_api.Models.Entities;
using AutoMapper;
using foodies_api.Interfaces.Services;
using System.Net;
using foodies_api.Models.Dtos.Responses;
using foodies_api.Auth;
using foodies_api.Models.Dtos.Requests;

namespace foodies_api.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IAuthRepository _authRepository;
        private IMapper _mapper;
        private IConfiguration _config;
        public AuthService(ILogger<AuthService> logger, IAuthRepository authRepository, IConfiguration config, IMapper mapper)
        {
            _logger = logger;
            _authRepository = authRepository;
            _config = config;
            _mapper = mapper;
        }

        public async Task<ApiResult<RegistrationResponse>> Register(RegistrationRequest request)
        {
                var user = _mapper.Map<User>(request);
                var result = await _authRepository.Register(user);
                
                if(!result.Success)
                {
                    _logger.LogError($"Error occurred during user registration.");
                    return new ApiResult<RegistrationResponse> { IsSuccess = false, Message = "User was not registered." };
                }
                
                _logger.LogInformation($"Registered User {result.Data.Username} successfully.");
                var response = _mapper.Map<RegistrationResponse>(result.Data);
                response.Token = new Authentication(_config).CreateAccessToken(user);

                return new ApiResult<RegistrationResponse> { Data = response, IsSuccess = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<ApiResult<LoginResponse>> Login(LoginRequest request)
        {
                var result = await _authRepository.Login(request.Email, request.Username, request.Password);
                
                if(!result.Success)
                {
                    _logger.LogError($"Error occurred during user login.");
                    return new ApiResult<LoginResponse> { IsSuccess = false, Message = "User was not logged into account." };
                }
                
                _logger.LogInformation($"Logged in User {result.Data.Username} account successfully.");
                var response = _mapper.Map<LoginResponse>(result.Data);
                response.Token = new Authentication(_config).CreateAccessToken(result.Data);
               
                return new ApiResult<LoginResponse> { Data = response, IsSuccess = true, StatusCode = HttpStatusCode.OK };
        }
    }
}