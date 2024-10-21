using System;
using foodies_api.Data;
using foodies_api.Interfaces.Repositories;
using foodies_api.Models.Dtos.Responses;
using Microsoft.EntityFrameworkCore;

namespace foodies_api.Repositories;

public class BusinessRepository(AppDbContext context) : IBusinessRepository
{
    public AppDbContext _context { get; set; } = context;
    public ILogger _logger { get; set; }

    public async Task<RepositoryResponse<Business>> AddBusiness(Business business) 
    {
        var businessExists = await _context.Businesses.AnyAsync(b => b.Id == business.Id );
        
        if(businessExists)
            return new RepositoryResponse<Business>() 
            {
                Success = true, 
                Data = business, 
                Message = "Business already exists" 
            }; 
            
        try {
                var result = await _context.Businesses.AddAsync(business);
                
                return new RepositoryResponse<Business>() { Success = true, Data = business };
        } 
        catch (Exception ex) 
        {
                return new RepositoryResponse<Business>() { Success = false, Exception = ex };
        }
    }
}
