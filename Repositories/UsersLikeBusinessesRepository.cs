using foodies_api.Data;
using foodies_api.Interfaces.Repositories;
using foodies_api.Models;
using foodies_api.Models.Dtos.Responses;
using Microsoft.EntityFrameworkCore;

namespace foodies_api.Repositories;

public class UsersLikeBusinessesRepository : IUsersLikeBusinessesRepository
{
    public AppDbContext _context { get; set; }
    public ILogger _logger { get; set; }
    public UsersLikeBusinessesRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<RepositoryResponse<UserLikeBusiness>> AddUserLikes(UserLikeBusiness userLike) 
    {
        try {
                var result = await _context.userLikeBusinesses.AddAsync(userLike);
                return new RepositoryResponse<UserLikeBusiness>() { Success = true, Data = userLike, Exception = null };

        } catch (Exception ex) {
                return new RepositoryResponse<UserLikeBusiness>() { Success = false, Exception = ex };
        }
    }

    public async Task<RepositoryResponse<UserLikeBusiness>> RemoveUserLikes(int userLikeId) 
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
    }
    
    public async Task<RepositoryResponse<List<UserLikeBusiness>>> GetUserLikes(Guid userId) 
    {
        try {
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                    throw new KeyNotFoundException("User not found");

                var userLikes = await _context.userLikeBusinesses
                    .Where(u => u.UserId == userId)
                    .ToListAsync();                

                return new RepositoryResponse<List<UserLikeBusiness>>() {
                     Success = true, 
                     Data = userLikes
                };

        } catch (Exception ex) {
                return new RepositoryResponse<List<UserLikeBusiness>>() { 
                    Success = false, 
                    Exception = ex 
                };
        }
    }
}
