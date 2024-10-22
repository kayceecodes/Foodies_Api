using Newtonsoft.Json;
using foodies_api.Interfaces.Services;
using foodies_api.Models;
using foodies_api.Interfaces.Repositories;
using System.Net;
using foodies_api.Models.Dtos.Responses;

namespace foodies_api.Services;

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
                ErrorMessages = ["Coundn't get any Businesses"] 
            };

        return new ApiResult<GetBusinessResponse> { Data = business, IsSuccess = true, StatusCode = HttpStatusCode.OK };
    }
}