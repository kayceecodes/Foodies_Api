using System;
using foodies_api.Models;
using foodies_api.Models.Dtos;
using foodies_api.Models.Entities;

namespace foodies_api.Interfaces.Services;

public interface IUsersLikeBusinessesService
{
    public Task<ApiResult<UserLikeBusinessDto>> AddUserLikes(UserLikeBusinessDto dto);
    public Task<ApiResult<UserLikeBusinessDto>> RemoveUserLikes(UserLikeBusinessDto dto);
    public Task<ApiResult<List<UserLikeBusinessDto>>> GetUserLikesByUserId(Guid userId);
}
