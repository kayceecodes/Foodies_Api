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
    private IMapper _mapper { get; set; }
    private IUsersLikeBusinessesRepository _repository { get; set; }
    private ILogger _logger { get; set; }

    public UsersLikeBusinessesService(ILogger logger, IMapper mapper, IUsersLikeBussinessesRepository repository) 
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
        _repository = repository;
    }

    public async Task<ApiResult<UserLikeBusiness>> AddUserLikeBusiness(UserLikeBusinessDto dto)
    {
        var mapped = _mapper.Map<UserLikeBusiness>(dto);
        var UserLikeBusiness = await _context.userLikeBusinesses.AddAsync(
            new UserLikeBusiness() { 
                UserId = mapped.UserId, 
                FullName = mapped.FullName, 
                BusinessId = mapped.BusinessId, 
                BusinessName = mapped.BusinessName }
        );
        
        return new ApiResult<UserLikeBusiness> { Data = mapped, IsSuccess = true, StatusCode = HttpStatusCode.OK };
    }

    public async Task<ApiResult<UserLikeBusiness>> RemoveUserLikeBusiness(UserLikeBusiness dto)
    {
        var mapped = mapper.Map<GetUserLikeBusinessResponse>(dto).ToList();
        var UserLikeBusiness = 

        return await new ApiResult<UserLikeBusiness> { Data = mapped, IsSuccess = true, StatusCode = HttpStatusCode.OK };
    }
}
