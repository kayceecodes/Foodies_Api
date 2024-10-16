using System;
using foodies_api.Models.Dtos.Responses;

namespace foodies_api.Interfaces.Repositories;

public interface IBusinessRepository
{
    public Task<RepositoryResponse<Business>> AddBusiness(Business business);

}
