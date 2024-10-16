using Newtonsoft.Json;
using foodies_api.Interfaces.Services;
using foodies_api.Models;
using foodies_api.Interfaces.Repositories;
using System.Net;

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

    public async Task<ApiResult<Business>> GetBusinessById(string businessId)
    {
        var httpClient = _httpClientFactory.CreateClient("FoodiesYelpService"); 
        var result = await httpClient.GetAsync(httpClient.BaseAddress + "/business/" + businessId);

        var business = JsonConvert.DeserializeObject<Business>(await result.Content.ReadAsStringAsync());

        if(!result.IsSuccessStatusCode)
            return new ApiResult<Business> 
            { 
                IsSuccess = false, 
                StatusCode = HttpStatusCode.BadRequest, 
                ErrorMessages = ["Coundn't get any Businesses"] 
            };

        return new ApiResult<Business> { Data = business, IsSuccess = true, StatusCode = HttpStatusCode.OK };
    }
}