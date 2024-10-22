using System;
using foodies_api.Models.Dtos.Responses;
using foodies_api.Models.Entities;

namespace foodies_api.Interfaces.Repositories;

public interface IBusinessRepository
{
    public Task<RepositoryResponse<Business>> AddBusiness(Business business);

}
