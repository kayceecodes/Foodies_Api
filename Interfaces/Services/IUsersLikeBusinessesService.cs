using System;
using foodies_api.Models;
using foodies_api.Models.Dtos;

namespace foodies_api.Interfaces.Services;

public interface IUsersLikeBusinessesService
{
    public Task<ApiResult<UserLikeBusiness>> AddUserLikeBusiness(UserLikeBusinessDto dto);
    public Task<ApiResult<UserLikeBusiness>> RemoveUserLikeBusiness(UserLikeBusinessDto dto);
}
