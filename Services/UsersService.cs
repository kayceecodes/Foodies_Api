using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using foodies_api.Interfaces.Repositories;
using foodies_api.Interfaces.Services;
using foodies_api.Models;
using foodies_api.Models.Dtos.Requests;
using foodies_api.Models.Entities;

namespace foodies_api.Services
{
    public class UsersService : IUsersService
    {
        private readonly ILogger<UsersService> _logger;
        private readonly IMapper _mapper;
        private IUsersRepository _repository;
        public UsersService(ILogger<UsersService> logger, IMapper mapper, IUsersRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ApiResult<List<User>>> GetUsers()
        {
            var result = await _repository.GetUsers();

            if(!result.Success)
            {
                _logger.LogError($"Failed to get users");
                return new ApiResult<List<User>> 
                { 
                    IsSuccess = false, 
                    StatusCode = HttpStatusCode.BadRequest, 
                    ErrorMessages = [$"Couldn't get any Users"],
                    Exception = result.Exception 
                };
            }

            _logger.LogInformation($"Successfully got users");
            return new ApiResult<List<User>> { Data = result.Data, IsSuccess = true, StatusCode = HttpStatusCode.OK };
        }

        public async Task<ApiResult<User>> GetUser(Guid userId)
        {
            var result = await _repository.GetUser(userId);

            if(!result.Success)
            {
                _logger.LogError($"Failed to get user with ID {userId}");
                return new ApiResult<User> 
                { 
                    IsSuccess = false, 
                    StatusCode = HttpStatusCode.BadRequest, 
                    ErrorMessages = [$"Couldn't get user with ID {userId}"]
                };
            }

            _logger.LogInformation($"Successfully got user with ID {userId}");
            return await Task.FromResult(new ApiResult<User>());
        }

        public async Task<ApiResult<User>> DeleteUser(Guid userId)
        {
            var result = await _repository.DeleteUser(userId);

            if(!result.Success)
            {
                _logger.LogError($"Failed to delete user with ID {userId}");
                return new ApiResult<User> 
                { 
                    IsSuccess = false, 
                    StatusCode = HttpStatusCode.BadRequest, 
                    ErrorMessages = [$"Couldn't delete user with ID {userId}"] 
                };
            }

            _logger.LogInformation($"Successfully deleted user with ID {userId}");
            return await Task.FromResult(new ApiResult<User>());
        }

        public async Task<ApiResult<User>> UpdateUser(Guid userId, UserUpdateRequest request)
        {
            var result = await _repository.UpdateUser(userId, request);

            if(!result.Success)
            {
                _logger.LogError($"Failed to update user with ID {userId}");
                return new ApiResult<User> 
                { 
                    IsSuccess = false, 
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = [$"Unable to update user with user ID {userId}"] 
                };
            }

            _logger.LogInformation($"Successfully updated user with ID {userId}");
            return await Task.FromResult(new ApiResult<User>());
        }
    }
}