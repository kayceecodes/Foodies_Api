using foodies_api.Data;
using foodies_api.Interfaces.Repositories;
using foodies_api.Models.Dtos.Requests;
using foodies_api.Models.Dtos.Responses;
using foodies_api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace foodies_api.Repositories
{
    /// <summary>
    /// Provides repository methods for managing user entities in the database.
    /// </summary>
    public class AuthRepository: IAuthRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UsersRepository> _logger;

        public AuthRepository(AppDbContext context, ILogger<UsersRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<RepositoryResponse<User>> Login(LoginRequest request)
        {
            var matchedUser = new User();

            try
            {
                if (!request.Email.IsNullOrEmpty())
                    matchedUser = await _context.Users
                    .Where(u => u.Email == request.Email && u.Password == request.Password)
                    .FirstOrDefaultAsync();
                else
                    matchedUser = await _context.Users
                    .Where(u => u.Username == request.Username && u.Password == request.Password)
                    .FirstOrDefaultAsync();

                if (matchedUser == null)
                {
                    _logger.LogError("Can not find user");
                    return new RepositoryResponse<User>() { 
                        Data = null, 
                        Success = false, 
                        Message = "User email/password does not match", 
                        Errors = new() {"Cannot find user by email or username"}
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to log in user");
                return new RepositoryResponse<User>()
                {
                    Success = false,
                    Exception = ex
                };
            }

            return new RepositoryResponse<User>() { Data = matchedUser, Success = true };  
        }

        public async Task<RepositoryResponse<User>> Register(User user)
        {
            bool usernameexists = await _context.Users.AnyAsync(u => u.Username == user.Username);
            bool emailexists = await _context.Users.AnyAsync(u => u.Email == user.Email);

            if (usernameexists)
                return new RepositoryResponse<User>()
                {
                    Success = false,
                    Message = "Username exists, choose another username",
                };

            if (emailexists)
                return new RepositoryResponse<User>()
                {
                    Success = false,
                    Message = "Email already exists, use another email",
                };

            user.Id = Guid.NewGuid();
            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return new RepositoryResponse<User>()
            {
                Data = user,
                Success = true,
                Message = "User successfully registered",
            };
        }
   }
}
