using Newtonsoft.Json;
using foodies_api.Interfaces.Services;
using foodies_api.Models;
using foodies_api.Interfaces.Repositories;
using System.Net;
using foodies_api.Models.Dtos.Responses;

namespace foodies_api.Services;

/// <summary>
/// Provides services for interacting with the Yelp API to retrieve business information.
/// </summary>
public class FoodiesYelpService : IFoodiesYelpService
{
    private ILogger<FoodiesYelpService> _logger; 
    private IHttpClientFactory _httpClientFactory;
    private IBusinessRepository _businessRepository;
    public FoodiesYelpService(ILogger<FoodiesYelpService> logger, IHttpClientFactory httpClientFactory, IBusinessRepository businessRepository)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _businessRepository = businessRepository;
    }

    /// <summary>
    /// Retrieves a business by its unique identifier from the Yelp API.
    /// </summary>
    /// <param name="businessId">The unique identifier of the business to retrieve.</param>
    /// <returns>
    /// An <see cref="ApiResult{T}"/> containing the business details if successful, 
    /// or an error message and status code if the operation fails.
    /// </returns>
    /// <exception cref="HttpRequestException">Thrown if the HTTP request to the Yelp API fails.</exception>
    public async Task<ApiResult<GetBusinessResponse>> GetBusinessById(string businessId)
    {
        var httpClient = _httpClientFactory.CreateClient("FoodiesYelpService"); 
        var result = await httpClient.GetAsync(httpClient.BaseAddress + "/business/" + businessId);

        var business = JsonConvert.DeserializeObject<GetBusinessResponse>(await result.Content.ReadAsStringAsync());

        if(!result.IsSuccessStatusCode)
            return new ApiResult<GetBusinessResponse> 
            { 
                IsSuccess = false, 
                StatusCode = HttpStatusCode.BadRequest, 
                Errors = ["Couldn't get any Businesses"] 
            };

        return new ApiResult<GetBusinessResponse> { Data = business, IsSuccess = true, StatusCode = HttpStatusCode.OK };
    }
}