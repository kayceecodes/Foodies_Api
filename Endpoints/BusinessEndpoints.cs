using System;
using AutoMapper;
using foodies_api.Models;
using foodies_api.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace foodies_api.Endpoints;

public static class BusinessEndpoints
{
    public static void ConfigurationBusinessEndpoints(this WebApplication app) 
    {
        // Uses Search object with propeerties used in Yelp's API
        app.MapPost("/api/business/add/{dto}", async Task<IResult> ([FromServices] IMapper mapper, [AsParameters] BusinessDto dto) =>
        {
            // ApiResult<BusinessDto> result = await service.AddBusiness(dto);

            // if (result.IsSuccess)
            // {
            //     var mapped = result.Data.Select(x => mapper.Map<GetBusinessResponse>(x)).ToList();
            //     return TypedResults.Ok(mapped);
            // }
            return TypedResults.Ok("Authorized Call");
            // return TypedResults.BadRequest(result.ErrorMessages);

        }).WithName("GetBusinesssBySearchTerms").Accepts<BusinessDto>("application/json")
        .Produces<ApiResult<List<BusinessDto>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();
    }

}
