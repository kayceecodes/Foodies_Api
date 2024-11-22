using System.Net;
using AutoMapper;
using foodies_api.Interfaces.Repositories;
using foodies_api.Interfaces.Services;
using foodies_api.Models;
using foodies_api.Models.Dtos;
using foodies_api.Models.Entities;

namespace foodies_api.Services;

public class UsersLikeBusinessesService : IUsersLikeBusinessesService
{
    private IMapper _mapper { get; set; }
    private IUsersLikeBusinessesRepository _repository { get; set; }
    private readonly ILogger<UsersLikeBusinessesService> _logger;

    public UsersLikeBusinessesService(ILogger<UsersLikeBusinessesService> logger, IMapper mapper, IUsersLikeBusinessesRepository repository) 
    {
        _logger = logger;
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<ApiResult<UserLikeBusinessDto>> AddUserLikes(UserLikeBusinessDto dto)
    {
        var userLikeBusiness = _mapper.Map<UserLikeBusiness>(dto);
        var result = await _repository.AddUserLikes(userLikeBusiness);
        
        if(result.Success)
        {
            return ApiResult<UserLikeBusinessDto>.Pass(dto);
        }
        else {
            _logger.LogError("Could not add to UserLikeBusiness");
            return new ApiResult<UserLikeBusinessDto> { IsSuccess = false, ErrorMessages = ["Couldn't add UserLikeBusiness"]};
        }
    }
    public async Task<ApiResult<UserLikeBusinessDto>> RemoveUserLikes(UserLikeBusinessDto dto)
    {
        var result = await _repository.RemoveUserLikes(dto.UserId, dto.BusinessId);

        if(!result.Success)
        {
            var message = $"Coundn't delete {dto.BusinessName} from UserLikeBusinesses";
            _logger.LogError(message);
            return ApiResult<UserLikeBusinessDto>.Fail(message, HttpStatusCode.BadRequest);
        }

        return ApiResult<UserLikeBusinessDto>.Pass(dto);
    }
    public async Task<ApiResult<List<UserLikeBusinessDto>>> GetUserLikesByUserId(Guid userId)
    {
        var result = await _repository.GetUserLikesByUserId(userId);
        List<UserLikeBusinessDto> dtos = _mapper.Map<List<UserLikeBusinessDto>>(result.Data); 

        if(!result.Success)
        {
            _logger.LogError("Couldn't get any UserLikeBusinesses");
            return new ApiResult<List<UserLikeBusinessDto>> 
            { 
                IsSuccess = false, 
                StatusCode = HttpStatusCode.BadRequest, 
                ErrorMessages = ["Coundn't get any UserLikeBusinesses"] 
            };
        }

        return new ApiResult<List<UserLikeBusinessDto>> { Data = dtos, IsSuccess = true, StatusCode = HttpStatusCode.OK };
    }
}
