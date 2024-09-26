using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using AutoMapper;
using foodies_api.Interfaces.Repositories;
using foodies_api.Interfaces.Services;
using foodies_api.Models;
using foodies_api.Models.Dtos;
using Microsoft.Extensions.Logging;

namespace foodies_api.Services;

public class UsersLikeBusinessesService : IUsersLikeBusinessesService
{
    private IMapper _mapper { get; set; }
    private IUsersLikeBusinessesRepository _repository { get; set; }
    private readonly ILogger<UsersLikeBusinessesService> _logger;

    public UsersLikeBusinessesService(ILogger<UsersLikeBusinessesService> logger, IMapper mapper, IUsersLikeBusinessesRepository repository) 
    {
        _logger = logger;
        _mapper = mapper;;
        _repository = repository;
    }

    public async Task<ApiResult<UserLikeBusiness>> AddUserLikes(UserLikeBusinessDto dto)
    {
        var userLikeBusiness = _mapper.Map<UserLikeBusiness>(dto);
        var result = await _repository.AddUserLikes(userLikeBusiness);

        if(result.Success)
        {
            return new ApiResult<UserLikeBusiness> { Data = result.Data, IsSuccess = true, StatusCode = HttpStatusCode.OK };
        }
        else {
            _logger.LogError("Could not add to UserLikeBusiness");
            return new ApiResult<UserLikeBusiness> { IsSuccess = false, ErrorMessages = ["Couldn't add UserLikeBusiness"]};
        }
    }
    public async Task<ApiResult<UserLikeBusiness>> RemoveUserLikes(UserLikeBusinessDto dto)
    {
        var result = await _repository.RemoveUserLikes(dto.UserId, dto.BusinessId);

        if(!result.Success)
            return new ApiResult<UserLikeBusiness> 
            { 
                IsSuccess = false, 
                StatusCode = HttpStatusCode.BadRequest, 
                ErrorMessages = ["Coundn't get any UserLikeBusinesses"] 
            };

        return new ApiResult<UserLikeBusiness> { Data = result.Data, IsSuccess = true, StatusCode = HttpStatusCode.OK };
    }

    public async Task<ApiResult<List<UserLikeBusiness>>> GetUserLikesByUserId(Guid userId)
    {
        var result = await _repository.GetUserLikesByUserId(userId);

        if(!result.Success)
            return new ApiResult<List<UserLikeBusiness>> 
            { 
                IsSuccess = false, 
                StatusCode = HttpStatusCode.BadRequest, 
                ErrorMessages = ["Coundn't get any UserLikeBusinesses"] 
            };

        return new ApiResult<List<UserLikeBusiness>> { Data = result.Data, IsSuccess = true, StatusCode = HttpStatusCode.OK };
    }
}
