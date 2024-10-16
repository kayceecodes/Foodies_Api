using System;
using foodies_api.Models;
using foodies_api.Models.Dtos;

namespace foodies_api.Interfaces.Services;

public interface IBusinessService
{
public Task<ApiResult<Business>> AddBusiness(GetBusinessResponse business);
}
