using System.Collections.Generic;
using System.Threading.Tasks;
using foodies_api.Models;
using foodies_api.Models.Dtos.Requests;
using foodies_api.Models.Dtos.Responses;
using foodies_api.Models.Entities;

namespace foodies_api.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        public Task<RepositoryResponse<List<User>>> GetAllUsers();
        public Task<RepositoryResponse<User>> GetUserById(Guid userId);
        public Task<RepositoryResponse<User>> UpdateUser(Guid userId, UserUpdateRequest request);
        public Task<RepositoryResponse<User>> DeleteUserById(Guid userId);
    }
}