using System;
using System.Net;
using AutoMapper;
using foodies_api.Interfaces.Repositories;
using foodies_api.Interfaces.Services;
using foodies_api.Models;
using foodies_api.Models.Dtos;
using Microsoft.AspNetCore.DataProtection.Repositories;

namespace foodies_api.Services;

public class BusinessService : IBusinessService
{
    private IMapper _mapper { get; set; }
    private IBusinessRepository _repository { get; set; }
    private readonly ILogger<BusinessService> _logger;

    public BusinessService(IBusinessRepository repository) 
    {
        _repository = repository;
    }

    public async Task<ApiResult<Business>> AddBusiness(GetBusinessResponse business)
    {
        var mappedBusiness = _mapper.Map<Business>(business);
        var result = await _repository.AddBusiness(mappedBusiness);

        if(!result.Success)
            return new ApiResult<Business> 
            { 
                IsSuccess = false, 
                StatusCode = HttpStatusCode.BadRequest, 
                ErrorMessages = ["Coundn't get any UserLikeBusinesses"] 
            };

        return new ApiResult<Business> { Data = result.Data, IsSuccess = true, StatusCode = HttpStatusCode.OK };
    }
}
