using foodies_api.Models.Dtos.Responses;
using foodies_api.Models.Dtos.Requests;
using foodies_api.Models.Entities;

namespace foodies_api.Interfaces.Repositories;

/// <summary>
/// Interface for the authentication repository, providing methods for user login and registration.
/// </summary>
public interface IAuthRepository
{
    Task<RepositoryResponse<User>> Login(LoginRequest loginRequest);
    Task<RepositoryResponse<User>> Register(User user);
}