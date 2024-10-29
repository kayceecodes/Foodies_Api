using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using foodies_api.Interfaces.Services;
using foodies_api.Models;
using foodies_api.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
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
        
            ApiResult<UserLikeBusinessDto> result = await usersLikeService.AddUserLikes(
                new UserLikeBusinessDto() {
                    UserId = Guid.Parse(httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                    BusinessId = foodiesYelpResult.Data.Id,
                    BusinessName = foodiesYelpResult.Data.Name,
                    Username = httpContext.User.FindFirst(JwtRegisteredClaimNames.Name).Value
                }
            );

            if (!result.IsSuccess)
                return TypedResults.BadRequest(result.ErrorMessages);

            return TypedResults.Ok($"Added NAME: {result.Data.BusinessName}, ID: {result.Data.BusinessId} to Businesses.");

        }).WithName("AddUsersLikeBusinessses").Accepts<UserLikeBusinessDto>("application/json")
        .Produces<ApiResult<List<UserLikeBusinessDto>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapDelete("/api/userlikebusinesses/{businessId}", [Authorize] async Task<IResult> (
            string businessId, 
            HttpContext httpContext,
            IUsersLikeBusinessesService service) =>
        {
            var userId = Guid.Parse(httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userLikeBusinessDto = new UserLikeBusinessDto() { UserId = userId, BusinessId = businessId };
            ApiResult<UserLikeBusinessDto> result = await service.RemoveUserLikes(userLikeBusinessDto);

            if (!result.IsSuccess)
                return TypedResults.BadRequest(result.ErrorMessages);
    
            return TypedResults.Ok<ApiResult<UserLikeBusinessDto>>(result);

        }).WithName("RemoveUsersLikeBusinessse").Accepts<UserLikeBusinessDto>("application/json")
        .Produces<ApiResult<List<UserLikeBusinessDto>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet("/api/userlikebusinesses/", [Authorize] async Task<IResult> (HttpContext httpContext, IUsersLikeBusinessesService service) =>
        {
            string userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userGuid = Guid.Parse(userId);
            
            ApiResult<List<UserLikeBusinessDto>> result = await service.GetUserLikesByUserId(userGuid);

            if(!result.IsSuccess)
                return TypedResults.BadRequest(result.ErrorMessages);

            return TypedResults.Ok<ApiResult<List<UserLikeBusinessDto>>>(result);

        }).WithName("GetUserLikeBusinesses").Accepts<UserLikeBusinessDto>("application/json")
        .Produces<ApiResult<List<UserLikeBusinessDto>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);    
    }
}
