using System;
using foodies_api.Data;
using foodies_api.Models;
using foodies_api.Models.Dtos.Responses;

namespace foodies_api.Interfaces.Repositories;

public interface IUsersLikeBusinessesRepository
{
    public Task<RepositoryResponse<UserLikeBusiness>> AddUserLikeBusiness(UserLikeBusiness userLikeBusiness);
   
    public Task<RepositoryResponse<UserLikeBusiness>> RemoveUserLikeBusiness(int id);
}
