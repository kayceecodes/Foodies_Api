using System;
using foodies_api.Data;
using foodies_api.Interfaces.Repositories;
using foodies_api.Models.Dtos.Responses;
using foodies_api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace foodies_api.Repositories;

/// <summary>
/// Provides repository methods for managing business entities in the database.
/// </summary>
public class BusinessRepository(AppDbContext context) : IBusinessRepository
{
    public AppDbContext _context { get; set; } = context;
    public ILogger _logger { get; set; }

    /// <summary>
    /// Adds a new business to the database.
    /// </summary>
    /// <param name="business">The <see cref="Business"/> object containing the business details to add.</param>
    /// <returns>
    /// A <see cref="RepositoryResponse{T}"/> containing the added business if successful, 
    /// or an error message and exception if the operation fails.
    /// </returns>
    public async Task<RepositoryResponse<Business>> AddBusiness(Business business) 
    {
        var businessExists = await _context.Businesses.AnyAsync(b => b.Id == business.Id ); //TODO: See why businessExists is always true
        
        if(businessExists)
            return new RepositoryResponse<Business>() 
            {
                Success = false, 
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
