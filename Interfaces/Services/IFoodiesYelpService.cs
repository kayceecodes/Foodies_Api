using System;
using foodies_api.Models;

namespace foodies_api.Interfaces.Services;

public interface IFoodiesYelpService
{
    public Task<ApiResult<Business>> GetBusinessById(string businessId);
}
