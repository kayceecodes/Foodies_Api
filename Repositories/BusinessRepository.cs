using System;
using foodies_api.Data;
using foodies_api.Interfaces.Repositories;
using foodies_api.Models.Dtos.Responses;

namespace foodies_api.Repositories;

public class BusinessRepository(AppDbContext context) : IBusinessRepository
{
    public AppDbContext _context { get; set; } = context;
    public ILogger _logger { get; set; }

    public async Task<RepositoryResponse<Business>> AddBusiness(Business business) 
    {
        var businessResult = _context.Businesses.FindAsync(business.Id);
        
        if(businessResult != null)
            return new RepositoryResponse<Business>() { Success = true, Message = "Business already exists" };

        try {
                var result = await _context.Businesses.AddAsync(business);
                await _context.SaveChangesAsync();
                
                return new RepositoryResponse<Business>() { Success = true, Data = business, Exception = null };
        } 
        catch (Exception ex) 
        {
                return new RepositoryResponse<Business>() { Success = false, Exception = ex };
        }
    }
}
