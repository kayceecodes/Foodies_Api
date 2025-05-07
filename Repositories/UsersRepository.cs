using System;
using foodies_api.Data;
using foodies_api.Interfaces.Repositories;
using foodies_api.Models.Dtos.Requests;
using foodies_api.Models.Dtos.Responses;
using foodies_api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace foodies_api.Repositories
{
    /// <summary>
    /// Provides repository methods for managing user entities in the database.
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UsersRepository> _logger;

        public UsersRepository(AppDbContext context, ILogger<UsersRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all users from the database.
        /// </summary>
        /// <returns>
        /// A <see cref="RepositoryResponse{T}"/> containing a list of users if successful
        /// </returns>
        public async Task<RepositoryResponse<List<User>>> GetAllUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();

                return new RepositoryResponse<List<User>>()
                {
                    Success = true,
                    Data = users
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get users");
                return new RepositoryResponse<List<User>>()
                {
                    Success = false,
                    Exception = ex
                };
            }
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve.</param>
        /// <returns>
        /// A <see cref="RepositoryResponse{T}"/> containing the user if found
        /// </returns>
        public async Task<RepositoryResponse<User>> GetUserById(Guid userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {userId} not found");
                    return new RepositoryResponse<User>()
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }
                return new RepositoryResponse<User>()
                {
                    Success = true,
                    Data = user
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get user with ID {userId}");
                return new RepositoryResponse<User>()
                {
                    Success = false,
                    Exception = ex
                };
            }
        }

        /// <summary>
        /// Deletes a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to delete.</param>
        /// <returns>
        /// A <see cref="RepositoryResponse{T}"/> containing the deleted user if successful, 
        /// </returns>
        /// <exception cref="Exception">Thrown if the user does not exist.</exception>
        public async Task<RepositoryResponse<User>> DeleteUserById(Guid userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();

                    return new RepositoryResponse<User>()
                    {
                        Success = true,
                        Data = user,
                    };
                }
                else 
                {
                    return new () {
                        Success = false,
                        Message = "User Not Found!" 
                    };
                }
           }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete user with ID {userId}");
                return new RepositoryResponse<User>() { Success = false, Exception = ex };
            }
        }

        /// <summary>
        /// Updates a user's information in the database.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to update.</param>
        /// <param name="request">The <see cref="UserUpdateRequest"/> containing the updated user information.</param>
        /// <returns>
        /// A <see cref="RepositoryResponse{T}"/> containing the updated user if successful
        /// </returns>
        public async Task<RepositoryResponse<User>> UpdateUser(Guid userId, UserUpdateRequest request)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {userId} not found");
                    return new RepositoryResponse<User>()
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                _context.Entry(user).CurrentValues.SetValues(request);
                await _context.SaveChangesAsync();

                return new RepositoryResponse<User>()
                {
                    Success = true,
                    Data = user
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update user with ID {userId}");
                return new RepositoryResponse<User>()
                {
                    Success = false,
                    Exception = ex
                };
            }
        }
    }
}
