using System.Net;
using AutoMapper;
using foodies_api.Interfaces.Repositories;
using foodies_api.Interfaces.Services;
using foodies_api.Models;
using foodies_api.Models.Dtos.Responses;
using foodies_api.Models.Entities;
using Microsoft.Extensions.Logging;

namespace foodies_api.Services;

public class BusinessService : IBusinessService
{
    private IMapper _mapper { get; set; }
    private IBusinessRepository _repository { get; set; }
    private readonly ILogger<BusinessService> _logger;

    public BusinessService(IBusinessRepository repository, IMapper mapper, ILogger<BusinessService> logger) 
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResult<Business>> AddBusiness(GetBusinessResponse business)
    {
        var mappedBusiness = _mapper.Map<Business>(business);
        var result = await _repository.AddBusiness(mappedBusiness);

        if(!result.Success)
        {
            _logger.LogError("Failed to add a new business");
            return new ApiResult<Business> 
            { 
                IsSuccess = false, 
                StatusCode = HttpStatusCode.BadRequest, 
                ErrorMessages = ["Couldn't add the business"] 
            };
        }
        _logger.LogInformation("Successfully added a new business");
        return new ApiResult<Business> { Data = result.Data, IsSuccess = true, StatusCode = HttpStatusCode.OK };
    }
}
