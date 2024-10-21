using System.Net;
using Newtonsoft.Json;
using System.Security.Claims;
using AutoMapper;
using foodies_api.Interfaces.Services;
using foodies_api.Models;
using foodies_api.Models.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace foodies_api.Endpoints;

public static class UserLikeBusinessEndpoints
{
    public static void ConfigurationUserLikeBusinessEndpoints(this WebApplication app) 
    {
        app.MapPost("/api/userlikebusinesses/", [Authorize] async Task<IResult> (
            [FromBody] UserLikeBusinessDto dto, 
            IUsersLikeBusinessesService usersLikeService, 
            HttpContext httpContext, 
            IFoodiesYelpService foodiesYelpService,
            IBusinessService businessService
             ) =>
        {
            var foodiesYelpResult = await foodiesYelpService.GetBusinessById(dto.BusinessId);
            await businessService.AddBusiness(foodiesYelpResult.Data);
        
            ApiResult<UserLikeBusiness> result = await usersLikeService.AddUserLikes(
                new UserLikeBusinessDto() {
                    UserId = Guid.Parse(httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                    BusinessId = foodiesYelpResult.Data.Id,
                    BusinessName = foodiesYelpResult.Data.Name,
                    FullName = httpContext.User.FindFirst(ClaimTypes.Name).Value
                }
            );

            if (!result.IsSuccess)
                return TypedResults.BadRequest(result.ErrorMessages);

            return TypedResults.Ok($"Added NAME: {result.Data.BusinessName}, ID: {result.Data.BusinessId} to Businesses.");

        }).WithName("AddUsersLikeBusinessses").Accepts<UserLikeBusinessDto>("application/json")
        .Produces<ApiResult<List<UserLikeBusinessDto>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapDelete("/api/userlikebusinesses/", [Authorize] async Task<IResult>  ([FromServices] IMapper mapper, [FromBody] UserLikeBusinessDto dto, IUsersLikeBusinessesService service) =>
        {
            ApiResult<UserLikeBusiness> result = await service.RemoveUserLikes(dto);

            if (!result.IsSuccess)
                return TypedResults.BadRequest(result.ErrorMessages);
    
            return TypedResults.Ok<ApiResult<UserLikeBusiness>>(result);

        }).WithName("RemoveUsersLikeBusinessse").Accepts<UserLikeBusinessDto>("application/json")
        .Produces<ApiResult<List<UserLikeBusinessDto>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet("/api/userlikebusinesses/", [Authorize] async Task<IResult> ([FromServices] IMapper mapper, HttpContext httpContext, IUsersLikeBusinessesService service) =>
        {
            string userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userGuid = Guid.Parse(userId);
            
            ApiResult<List<UserLikeBusiness>> result = await service.GetUserLikesByUserId(userGuid);

            if(!result.IsSuccess)
                return TypedResults.BadRequest(result.ErrorMessages);

            return TypedResults.Ok<ApiResult<List<UserLikeBusiness>>>(result);

        }).WithName("GetUserLikeBusinesses").Accepts<UserLikeBusinessDto>("application/json")
        .Produces<ApiResult<List<UserLikeBusinessDto>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);    
    }
}
