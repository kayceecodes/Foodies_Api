using System.Security.Claims;
using foodies_api.Data;
using foodies_api.Interfaces.Repositories;
using foodies_api.Models;
using foodies_api.Models.Dtos;
using foodies_api.Models.Dtos.Responses;
using foodies_api.Models.Entities;
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
                var result = await _context.UserLikeBusinesses.AddAsync(userLike);
                await _context.SaveChangesAsync();
                
                return new RepositoryResponse<UserLikeBusiness>() { Success = true, Data = userLike, Exception = null };
        } 
        catch (Exception ex) 
        {
                return new RepositoryResponse<UserLikeBusiness>() { Success = false, Exception = ex };
        }
    }

    public async Task<RepositoryResponse<UserLikeBusiness>> RemoveUserLikes(Guid userId, string businessId) 
    { 
        UserLikeBusiness userLike = await _context.UserLikeBusinesses.FindAsync(userId, businessId);
        
        if(userLike == null)
           return new RepositoryResponse<UserLikeBusiness>() 
            { 
                    Success = true, 
                    Exception = new KeyNotFoundException($"Entity of type UserLikeBusiness could not be found.")
            };            

        try {
                var result =  _context.UserLikeBusinesses.Remove(userLike); 
                await _context.SaveChangesAsync();
                return new RepositoryResponse<UserLikeBusiness>() 
                {
                     Success = true, 
                     Exception = null, 
                     Data = userLike,
                     Message = $"Userlikebusiness {userLike.FullName}, {userLike.BusinessName} deleted." 
                };

        }
        catch (Exception ex) 
        {
                return new RepositoryResponse<UserLikeBusiness>() { Success = false, Exception = ex };
        }
    }
    
    public async Task<RepositoryResponse<List<UserLikeBusiness>>> GetUserLikesByUserId(Guid userId) 
    {        
        try
        {           
                var userLikes = await _context.UserLikeBusinesses
                    // .Where(ub => ub.User.Id.Equals(userId))
                    .ToListAsync();                

                return new RepositoryResponse<List<UserLikeBusiness>>() 
                {
                     Success = true, 
                     Data = userLikes
                };

        } 
        catch (Exception ex) 
        {
                return new RepositoryResponse<List<UserLikeBusiness>>() 
                { 
                    Success = false, 
                    Exception = ex 
                };
        }
    }
}
