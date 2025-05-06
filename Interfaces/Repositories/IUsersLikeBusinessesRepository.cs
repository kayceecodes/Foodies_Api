using foodies_api.Models.Dtos.Responses;
using foodies_api.Models.Entities;

namespace foodies_api.Interfaces.Repositories;

public interface IUsersLikeBusinessesRepository
{
    public Task<RepositoryResponse<UserLikeBusiness>> AddUserLikes(UserLikeBusiness userLike);
   
    public Task<RepositoryResponse<UserLikeBusiness>> RemoveUserLikes(Guid userId, string businessId);
    
    public Task<RepositoryResponse<List<UserLikeBusiness>>> GetUserLikesByUserId(Guid userId);
}
