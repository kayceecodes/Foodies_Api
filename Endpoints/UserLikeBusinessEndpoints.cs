using System;
using AutoMapper;
using foodies_api.Data;
using foodies_api.Interfaces.Services;
using foodies_api.Models;
using foodies_api.Models.Dtos;
using foodies_api.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace foodies_api.Endpoints;

public static class UserLikeBusinessEndpoints
{
    public static void ConfigurationUserLikeBusinessEndpoints(this WebApplication app, AppDbContext context) 
    {
        // Uses Search object with propeerties used in Yelp's API
        app.MapPost("/api/userlikebusiness/add/{dto}", async Task<IResult> ([FromServices] IMapper mapper, [AsParameters] UserLikeBusinessDto dto) =>
        {
            IUsersLikeBusinessesService service = new UsersLikeBusinessesService(context);

            ApiResult<UserLikeBusiness> result = await service.AddUserLikeBusiness(dto);

            // if (result.IsSuccess)
            // {
            //     var mapped = result.Data.Select(x => mapper.Map<GetBusinessResponse>(x)).ToList();
            //     return TypedResults.Ok(mapped);
            // }
            return TypedResults.Ok("Authorized Call");
            // return TypedResults.BadRequest(result.ErrorMessages);

        }).WithName("GetUserLikeBusinesssBySearchTerms").Accepts<UserLikeBusinessDto>("application/json")
        .Produces<ApiResult<List<UserLikeBusinessDto>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();
    }

}
