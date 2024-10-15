using System.Net;
using Newtonsoft.Json;
using System.Security.Claims;
using AutoMapper;
using foodies_api.Interfaces.Services;
using foodies_api.Models;
using foodies_api.Models.Dtos;
using foodies_api.Services;
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
        app.MapPost("/api/userlikebusinesses/", [Authorize] async Task<IResult>  (
            [FromServices] IMapper mapper, 
            [FromBody] UserLikeBusinessDto dto, 
            IUsersLikeBusinessesService service, 
            HttpContext httpContext, 
            IHttpClientFactory clientFactory ) =>
        {
            var httpClient = clientFactory.CreateClient("FoodiesYelpService"); 
            var yelpResult = await httpClient.GetAsync(httpClient.BaseAddress + "/business/" + dto.BusinessId);
        
            if (!yelpResult.IsSuccessStatusCode)
                    throw new HttpRequestException("Failed to fetch business details from Foodies-Yelp API");

            var businessId = JsonConvert.DeserializeObject<GetBusinessResponse>(await yelpResult.Content.ReadAsStringAsync()).Id;

            ApiResult<UserLikeBusiness> result = await service.AddUserLikes(
                new UserLikeBusinessDto() {
                    UserId = Guid.Parse(httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                    BusinessId = businessId
                }
            );

            if (!result.IsSuccess)
                return TypedResults.BadRequest(result.ErrorMessages);

            return TypedResults.Ok("Authorized Call");

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
