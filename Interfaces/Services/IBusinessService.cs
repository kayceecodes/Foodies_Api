using System;
using foodies_api.Models;
using foodies_api.Models.Dtos.Responses;
using foodies_api.Models.Entities;

namespace foodies_api.Interfaces.Services;

public interface IBusinessService
{
public Task<ApiResult<Business>> AddBusiness(GetBusinessResponse business);
}
