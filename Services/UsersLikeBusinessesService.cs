using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using AutoMapper;
using foodies_api.Data;
using foodies_api.Interfaces.Services;
using foodies_api.Models;
using foodies_api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace foodies_api.Services;

public class UsersLikeBusinessesService : IUsersLikeBusinessesService
{
    private AppDbContext _context { get; set; }
    private IMapper mapper { get; set; }
    public UsersLikeBusinessesService(AppDbContext context) 
    {
        _context = context;
    }

    public async Task<ApiResult<UserLikeBusiness>> AddUserLikeBusiness(UserLikeBusinessDto dto, [FromServices] IMapper mapper)
    {
        var mapped = mapper.Map<GetUserLikeBusinessResponse>(dto).ToList();
        var UserLikeBusiness = _context.userLikeBusinesses.AddAsync(
            new UserLikeBusiness() { 
                UserId = mapped.UserId, 
                FullName = mapped.FirstAndLastName, 
                BusinessId = mapped.BusinessId, 
                BusinessName = mapped.BusinessName }
        );
        
        return await new ApiResult<UserLikeBusiness> { Data = mapped, IsSuccess = true, StatusCode = HttpStatusCode.OK };
    }

    public async Task<ApiResult<UserLikeBusiness>> RemoveUserLikeBusiness(UserLikeBusiness dto)
    {
        var mapped = mapper.Map<GetUserLikeBusinessResponse>(dto).ToList();
        var UserLikeBusiness = 

        return await new ApiResult<UserLikeBusiness> { Data = mapped, IsSuccess = true, StatusCode = HttpStatusCode.OK };
    }
}
