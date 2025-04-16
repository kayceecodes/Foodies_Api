using System;
using foodies_api.Models;
using foodies_api.Models.Dtos.Requests;
using foodies_api.Models.Dtos.Responses;
using foodies_api.Models.Entities;

namespace foodies_api.Interfaces.Services;

public interface IUsersService
{
    public Task<ApiResult<List<User>>> GetAllUsers();
    public Task<ApiResult<User>> GetUserById(Guid userId);
    public Task<ApiResult<User>> UpdateUser(Guid userId, UserUpdateRequest request);
    public Task<ApiResult<User>> DeleteUserById(Guid userId);
}
