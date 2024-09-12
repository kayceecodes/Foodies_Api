using foodies_api.Data;
using foodies_api.Interfaces.Repositories;
using foodies_api.Models;
using foodies_api.Models.Dtos.Responses;

namespace foodies_api.Repositories;

public class UsersLikeBusinessesRepository : IUsersLikeBusinessesRepository
{
    public AppDbContext _context { get; set; }
    public ILogger _logger { get; set; }
    public UsersLikeBusinessesRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<RepositoryResponse<UserLikeBusiness>> AddUserLikeBusiness(UserLikeBusiness userLike) 
    {
        try {
                var result = await _context.userLikeBusinesses.AddAsync(userLike);
                return new RepositoryResponse<UserLikeBusiness>() { Success = true, Data = userLike, Exception = null };

        } catch (Exception ex) {
                return new RepositoryResponse<UserLikeBusiness>() { Success = false, Exception = ex };
        }
    }

    public async Task<RepositoryResponse<UserLikeBusiness>> RemoveUserLikeBusiness(int userLikeId) 
    {
        var userLike = await _context.userLikeBusinesses.FindAsync(userLikeId);
        
        if(userLike == null)
           return new RepositoryResponse<UserLikeBusiness>() { 
                Success = true, 
                Exception = new KeyNotFoundException($"Entity of type UserLikeBusiness could not be found.")
            };            

        try {
                var result =  _context.userLikeBusinesses.Remove(userLike); 
                await _context.SaveChangesAsync();
                return new RepositoryResponse<UserLikeBusiness>() {
                     Success = true, 
                     Exception = null, 
                     Data = userLike,
                     Message = $"Userlikebusiness {userLike.FullName}, {userLike.BusinessName} deleted." 
                };

        } catch (Exception ex) {
                return new RepositoryResponse<UserLikeBusiness>() { Success = false, Exception = ex };
        }
    }}
