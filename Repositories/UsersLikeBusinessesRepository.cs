using System.Security.Claims;
using AutoMapper;
using foodies_api.Data;
using foodies_api.Interfaces.Repositories;
using foodies_api.Models;
using foodies_api.Models.Dtos;
using foodies_api.Models.Dtos.Responses;
using foodies_api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace foodies_api.Repositories;

/// <summary>
/// Provides repository methods for managing user-business like relationships in the database.
/// </summary>
public class UsersLikeBusinessesRepository: IUsersLikeBusinessesRepository
{
    public AppDbContext _context { get; set; }
    public ILogger<UserLikeBusiness> _logger { get; set; }
    public IMapper _mapper { get; set; }
    public UsersLikeBusinessesRepository(AppDbContext context, IMapper mapper, ILogger<UserLikeBusiness> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }
    
    /// <summary>
    /// Adds a new user-business like relationship to the database.
    /// </summary>
    /// <param name="userLike">The <see cref="UserLikeBusiness"/> object containing the user-business like details to add.</param>
    /// <returns>
    /// A <see cref="RepositoryResponse{T}"/> containing the added user-business like relationship if successful
    /// </returns>
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

    /// <summary>
    /// Removes a user-business like relationship from the database.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="businessId">The unique identifier of the business.</param>
    /// <returns>
    /// A <see cref="RepositoryResponse{T}"/> containing the removed user-business like relationship if successful
    /// </returns>
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
                     Message = $"Userlikebusiness {userLike.Username}, from {userLike.BusinessName} deleted." 
                };

        }
        catch (Exception ex) 
        {
                return new RepositoryResponse<UserLikeBusiness>() { Success = false, Exception = ex };
        }
    }
    
    /// <summary>
    /// Retrieves all user-business like relationships for a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>
    /// A <see cref="RepositoryResponse{T}"/> containing a list of user-business like relationships if successful
    /// </returns>
    public async Task<RepositoryResponse<List<UserLikeBusiness>>> GetUserLikesByUserId(Guid userId) 
    {        
        try
        {           
                var userLikes = await _context.UserLikeBusinesses
                    .Where(ub => ub.User.Id.Equals(userId))
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
