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
    /// <summary>
    /// Provides services for managing users, including retrieving, updating, and deleting user data.
    /// </summary>
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

        /// <summary>
        /// Retrieves a list of all users.
        /// </summary>
        /// <returns>
        /// An <see cref="ApiResult{T}"/> containing a list of users if successful, 
        /// </returns>
        public async Task<ApiResult<List<User>>> GetAllUsers()
        {
            var result = await _repository.GetAllUsers();

            if(!result.Success)
            {
                _logger.LogError($"Failed to get users");
                return new ApiResult<List<User>> 
                { 
                    IsSuccess = false, 
                    StatusCode = HttpStatusCode.BadRequest, 
                    ErrorMessages = [result.Message],
                    Exception = result.Exception 
                };
            }

            _logger.LogInformation($"Successfully got users");
            return new ApiResult<List<User>> { Data = result.Data, IsSuccess = true, StatusCode = HttpStatusCode.OK };
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.  
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve.</param>
        /// <returns>
        /// An <see cref="ApiResult{T}"/> containing the user if found
        /// </returns>
            public async Task<ApiResult<User>> GetUserById(Guid userId)
        {
            var result = await _repository.GetUserById(userId);

            if(!result.Success)
            {
                _logger.LogError($"Failed to get user with ID {userId}");
                return new ApiResult<User> 
                { 
                    IsSuccess = false, 
                    StatusCode = HttpStatusCode.BadRequest, 
                    ErrorMessages = [result.Message]
                };
            }

            _logger.LogInformation($"Successfully got user with ID {userId}");
            return new ApiResult<User> { Data = result.Data, IsSuccess = true, StatusCode = HttpStatusCode.OK };
        }

        /// <summary>
        /// Deletes a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to delete.</param>
        /// <returns>
        /// An <see cref="ApiResult{T}"/> containing the deleted user if successful
        /// </returns>
        public async Task<ApiResult<User>> DeleteUserById(Guid userId)
        {
            var result = await _repository.DeleteUserById(userId);

            if(!result.Success)
            {
                _logger.LogError($"Failed to delete user with ID {userId}");
                return new ApiResult<User> 
                { 
                    IsSuccess = false, 
                    StatusCode = HttpStatusCode.BadRequest, 
                    ErrorMessages = [result.Message] 
                };
            }

            _logger.LogInformation($"Successfully deleted user with ID {userId}");

            return new ApiResult<User> { Data = result.Data, IsSuccess = true, StatusCode = HttpStatusCode.OK };
        }

        /// <summary>
        /// Updates a user's information.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to update.</param>
        /// <param name="request">The <see cref="UserUpdateRequest"/> containing the updated user information.</param>
        /// <returns>
        /// An <see cref="ApiResult{T}"/> containing the updated user if successful
        /// </returns>
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
            return new ApiResult<User> { Data = result.Data, IsSuccess = true, StatusCode = HttpStatusCode.OK };
        }
    }
}