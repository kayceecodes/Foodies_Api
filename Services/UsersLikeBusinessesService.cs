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

    public async Task<ApiResult<UserLikeBusiness>> AddUserLikeBusiness(UserLikeBusinessDto dto)
    {
        var mapped = _mapper.Map<UserLikeBusiness>(dto);
        var result = await _repository.AddUserLikeBusiness(mapped);

        if(result.Success)
        {
            return new ApiResult<UserLikeBusiness> { Data = result.Data, IsSuccess = true, StatusCode = HttpStatusCode.OK };
        }
        else {
            _logger.LogError("Could not add to UserLikeBusiness");
            return new ApiResult<UserLikeBusiness> { IsSuccess = false, ErrorMessages = ["Couldn't add UserLikeBusiness"]};
        }
    }
    public async Task<ApiResult<UserLikeBusiness>> RemoveUserLikeBusiness(UserLikeBusinessDto dto)
    {
        var mapped = _mapper.Map<UserLikeBusiness>(dto);

        return new ApiResult<UserLikeBusiness> { Data = mapped, IsSuccess = true, StatusCode = HttpStatusCode.OK };
    }
}
