using System;
using foodies_api.Data;
using foodies_api.Models;
using foodies_api.Models.Dtos;
using foodies_api.Models.Dtos.Responses;
using foodies_api.Models.Entities;

namespace foodies_api.Interfaces.Repositories;

public interface IAuthRepository
{
    public Task<RepositoryResponse<User>> Register(User user);
    public Task<RepositoryResponse<User>> Login(string username, string email, string password);    
}
