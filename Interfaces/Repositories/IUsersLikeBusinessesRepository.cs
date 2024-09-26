using System;
using foodies_api.Data;
using foodies_api.Models;
using foodies_api.Models.Dtos;
using foodies_api.Models.Dtos.Responses;

namespace foodies_api.Interfaces.Repositories;

public interface IUsersLikeBusinessesRepository
{
    public Task<RepositoryResponse<UserLikeBusiness>> AddUserLikes(UserLikeBusiness userLike);
   
    public Task<RepositoryResponse<UserLikeBusiness>> RemoveUserLikes(Guid userId, string businessId);
    
    public Task<RepositoryResponse<List<UserLikeBusiness>>> GetUserLikesByUserId(Guid userId);
}
