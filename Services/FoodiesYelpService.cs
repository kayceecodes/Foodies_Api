using Newtonsoft.Json;
using System.Net.Http.Headers;
using foodies_yelp.Models.Dtos.Requests;
using foodies_yelp.Models.Dtos.Responses;
using foodies_yelp.Models.Dtos.Responses.Yelp;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace foodies_yelp.Services;

public class FoodiesYelpService : IFoodiesYelpService
{
    private ILogger<YelpService> _logger; 
    private IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public YelpService(ILogger<YelpService> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    private bool IsPlaceHolderString(string token)
    {
        string pattern = @"(API|SECRET|TOKEN)";
        
        Regex regex = new Regex(pattern);
        MatchCollection matches = regex.Matches(token);

        return matches.Count > 0;
    }

    public HttpClient CreateClient() 
    {
        var client = _httpClientFactory.CreateClient("YelpService");
        var token = string.Empty;
        
        try {
            token = _configuration[YelpConstants.ApiKeyName];
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        catch(UnauthorizedAccessException ex) {
            _logger.LogError(ex, "Cannot reach API token");
            throw new UnauthorizedAccessException("Cannot reach API token");
        }
        
        if(token.IsNullOrEmpty() || IsPlaceHolderString(token)) {
            _logger.LogError("API token is empty or is a placeholder");
            throw new UnauthorizedAccessException("API token is empty or is a placeholder");
        }

        return client;
    }

    public async virtual Task<APIResult<Business>> GetBusinessById(string id)
    {
        HttpClient client = CreateClient();
        string url = client.BaseAddress + $"/businesses/{id}";

        HttpResponseMessage result = await client.GetAsync(url);
        var business = JsonConvert.DeserializeObject<Business>(await result.Content.ReadAsStringAsync());

        if (result.IsSuccessStatusCode)
            return APIResult<Business>.Pass(business);
        else
            return APIResult<Business>.Fail($"Problem getting bussiness using Id: {id}", result.StatusCode);
    }
}