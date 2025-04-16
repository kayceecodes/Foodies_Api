using System.Net;
using AutoMapper;
using foodies_api.Interfaces.Repositories;
using foodies_api.Interfaces.Services;
using foodies_api.Models;
using foodies_api.Models.Dtos.Responses;
using foodies_api.Models.Entities;
using Microsoft.Extensions.Logging;

namespace foodies_api.Services;


/// <summary>
/// Provides services for managing businesses, including adding, retrieving, and updating business data.
/// </summary>
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

    /// <summary>
    /// Adds a new business to the system.
    /// </summary>
    /// <param name="business">The <see cref="GetBusinessResponse"/> object containing the business details to add.</param>
    /// <returns>
    /// An <see cref="ApiResult{T}"/> containing the added business if successful, 
    /// or an error message and status code if the operation fails.
    /// </returns>
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
