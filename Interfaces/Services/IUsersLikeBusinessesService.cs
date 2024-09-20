using System;
using foodies_api.Models;
using foodies_api.Models.Dtos;

namespace foodies_api.Interfaces.Services;

public interface IUsersLikeBusinessesService
{
    public Task<ApiResult<UserLikeBusiness>> AddUserLikes(UserLikeBusinessDto dto);
    public Task<ApiResult<UserLikeBusiness>> RemoveUserLikes(UserLikeBusinessDto dto);
    public Task<ApiResult<List<UserLikeBusiness>>> GetUserLikes(UserDto dto);
}
