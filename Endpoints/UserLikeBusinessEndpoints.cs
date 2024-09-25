using System.Net;
using AutoMapper;
using foodies_api.Interfaces.Services;
using foodies_api.Models;
using foodies_api.Models.Dtos;
using foodies_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace foodies_api.Endpoints;

public static class UserLikeBusinessEndpoints
{
    public static void ConfigurationUserLikeBusinessEndpoints(this WebApplication app) 
    {
        app.MapPost("/api/userlikebusinesses/", [Authorize] async Task<IResult>  ([FromServices] IMapper mapper, [FromBody] UserLikeBusinessDto dto, IUsersLikeBusinessesService service) =>
        {
            ApiResult<UserLikeBusiness> result = await service.AddUserLikes(dto);

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

        app.MapGet("/api/userlikebusinesses/", [Authorize] async Task<IResult> ([FromServices] IMapper mapper, [FromBody] UserDto dto, IUsersLikeBusinessesService service) =>
        {
            if(dto == null)
                return TypedResults.BadRequest(ApiResult<UserDto>.Fail("User object cannot be null", HttpStatusCode.BadRequest));

            ApiResult<List<UserLikeBusiness>> result = await service.GetUserLikes(dto.Username);

            if(!result.IsSuccess)
                return TypedResults.BadRequest(result.ErrorMessages);

            return TypedResults.Ok<ApiResult<List<UserLikeBusiness>>>(result);

        }).WithName("GetUserLikeBusinesses").Accepts<UserLikeBusinessDto>("application/json")
        .Produces<ApiResult<List<UserLikeBusinessDto>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);    
    }
}
