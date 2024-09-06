using System;
using foodies_api.Data;
using foodies_api.Models;

namespace foodies_api.Interfaces.Repositories;

public class IUsersLikeBusinessesRepository
{
    public AppDbContext _context { get; set; }
    IUsersLikeBusinessesRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<RepositoryResponse<UserLikeBusiness>> AddUserLikeBusiness()
    {
        return new RespositoryResponse<UserLikeBusiness>()
    }
}
