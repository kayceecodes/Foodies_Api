using System;
using foodies_api.Data;
using foodies_api.Models;
using foodies_api.Models.Dtos.Responses;

namespace foodies_api.Interfaces.Repositories;

public interface IUsersLikeBusinessesRepository
{
    public Task<RepositoryResponse<UserLikeBusiness>> AddUserLikes(UserLikeBusiness userLikeBusiness);
   
    public Task<RepositoryResponse<UserLikeBusiness>> RemoveUserLikes(int userLikeBusinessId);
    
    public Task<RepositoryResponse<List<UserLikeBusiness>>> GetUserLikes(Guid userId);
}
