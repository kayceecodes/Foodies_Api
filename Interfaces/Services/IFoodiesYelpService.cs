using System;
using foodies_api.Models;
using foodies_api.Models.Dtos;

namespace foodies_api.Interfaces.Services;

public interface IFoodiesYelpService
{
    public Task<ApiResult<GetBusinessResponse>> GetBusinessById(string businessId);
}