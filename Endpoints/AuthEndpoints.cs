using AutoMapper;
using foodies_api.Data;
using foodies_api.Models.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;
using foodies_api.Models;
using foodies_api.Models.Entities;
using foodies_api.Interfaces.Services;

namespace foodies_api.Endpoints;

public static class AuthEndpoints
{
    public static void ConfigAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/api/auth/login", async Task<IResult>
        ([FromBody] LoginRequest request, IAuthService service) =>
        {
            var result = await service.Login(request);

            if (!result.IsSuccess)
                return TypedResults.BadRequest(result);

            return TypedResults.Ok(result);
        })
        .WithName("Login")
        .Accepts<string>("application/json")
        .Produces<ApiResult<List<User>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithOpenApi();

        app.MapPost("/api/auth/register", async Task<IResult>
        ([FromBody] RegisterRequest request, IAuthService service, AppDbContext context, IConfiguration config, IMapper mapper) =>
        {
            var result = await service.Register(request);

            if (!result.IsSuccess)
                return TypedResults.BadRequest(result);

            return TypedResults.Ok(result);
        })
        .WithName("Register User")
        .Accepts<string>("application/json")
        .Produces<ApiResult<List<User>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithOpenApi();
    }
}
