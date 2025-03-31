using System;
using foodies_api.Data;
using foodies_api.Interfaces.Repositories;
using foodies_api.Models.Dtos.Requests;
using foodies_api.Models.Dtos.Responses;
using foodies_api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace foodies_api.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UsersRepository> _logger;

        public UsersRepository(AppDbContext context, ILogger<UsersRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<RepositoryResponse<List<User>>> GetUsers()
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

        public async Task<RepositoryResponse<User>> GetUser(Guid userId)
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
                        Exception = new Exception("User not found")
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

        public async Task<RepositoryResponse<User>> DeleteUser(Guid userId)
        {
            var userExists = await _context.Users.AnyAsync(b => b.Id == userId);

            if (!userExists)
            {
                _logger.LogWarning($"User with ID {userId} does not exist");
                throw new Exception("User does not exist");
            }

            try
            {
                var user = await _context.Users.FindAsync(userId);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return new RepositoryResponse<User>()
                {
                    Success = true,
                    Data = user,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete user with ID {userId}");
                return new RepositoryResponse<User>() { Success = false, Exception = ex };
            }
        }

        public async Task<RepositoryResponse<User>> UpdateUser(Guid userId, UserUpdateRequest request)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(userId);
                if (existingUser == null)
                {
                    _logger.LogWarning($"User with ID {userId} not found");
                    return new RepositoryResponse<User>()
                    {
                        Success = false,
                        Exception = new Exception("User not found")
                    };
                }

                _context.Entry(existingUser).CurrentValues.SetValues(request);
                await _context.SaveChangesAsync();

                return new RepositoryResponse<User>()
                {
                    Success = true,
                    Data = existingUser
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
