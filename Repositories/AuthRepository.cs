using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using foodies_api.Data;
using foodies_api.Models;
using foodies_api.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using foodies_api.Models.Dtos.Responses;
using foodies_api.Models.Entities;
using Microsoft.IdentityModel.Tokens;

namespace foodies_api.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AuthRepository> _logger;

        public AuthRepository(AppDbContext context, ILogger<AuthRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<RepositoryResponse<User>> Register(User user)
        {
            try
            {
                bool usernameExists = _context.Users.Any(u => u.Username == user.Username);
                bool emailExists = _context.Users.Any(u => u.Email == user.Email);

                if (usernameExists)
                {
                    _logger.LogWarning("Username already exists.");
                    return new RepositoryResponse<User>() { Success = false, Message = "Username already exists" };
                }

                if (emailExists)
                {
                    _logger.LogWarning("Email already exists.");
                    return new RepositoryResponse<User>() { Success = false, Message = "Email already exists" };
                }

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return new RepositoryResponse<User>() { Success = true, Data = user};
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering the user.");
                return new RepositoryResponse<User>() { Success = false, Exception = ex, Message = ex.Message };
            }
        }

        public async Task<RepositoryResponse<User>> Login(string username, string email, string password)
        {
            try
            {
                var matchedUser = new User();
                
                if (!email.IsNullOrEmpty())
                    matchedUser = _context.Users.Where(u => u.Email == email && u.Password == password).FirstOrDefault();
                else
                    matchedUser = _context.Users.Where(u => u.Username == username && u.Password == password).FirstOrDefault();

                if (matchedUser == null)
                    _logger.LogWarning("User not found.");

                return new RepositoryResponse<User>() { Success = true, Data = matchedUser, Message = "User found."};
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in the user.");
                return new RepositoryResponse<User>() { Success = false, Exception = ex, Message = ex.Message };
            }
        }
    }
}