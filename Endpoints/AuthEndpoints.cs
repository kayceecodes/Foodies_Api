using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using foodies_api.Data;
using foodies_api.Models.Dtos.Requests;
using foodies_api.Models;
using foodies_api.Models.Entities;
using foodies_api.Interfaces.Services;
using foodies_api.Models.Dtos.Responses;
using System.Net;

namespace foodies_api.Endpoints;

public static class AuthEndpoints
{
    public static void ConfigAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/api/login", async Task<IResult>
        ([FromBody] LoginRequest request, IAuthService service, HttpContext httpContext) =>
        {
            var result = await service.Login(request);
        
            if (!result.IsSuccess)
                return TypedResults.BadRequest(result);

            httpContext.Response.Cookies.Append("auth-token", result.Data.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            result.Data.Token = null;

            return TypedResults.Ok(result);
        })
        .WithName("Login")
        .Accepts<string>("application/json")
        .Produces<ApiResult<List<User>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithOpenApi();

        app.MapPost("/api/register", async Task<IResult>
        ([FromBody] RegisterRequest request, IAuthService service, HttpContext httpContext) =>
        {
            var result = await service.Register(request);

            if (!result.IsSuccess)
                return TypedResults.BadRequest(result);

            // Set HttpOnly cookie (auto-login after registration)
            httpContext.Response.Cookies.Append("auth-token", result.Data.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            result.Data.Token = null;
            
            return TypedResults.Ok(result);
        })
        .WithName("Register")
        .Accepts<string>("application/json")
        .Produces<ApiResult<List<User>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithOpenApi();

        app.MapPost("/api/logout", async Task<IResult>
        ([FromBody] IAuthService service, HttpContext context) =>
        {
            context.Response.Cookies.Delete("auth-token", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/" // recommended to include explicitly
            });

            var result = new ApiResult<LogoutResponse>() { Message = "Logged out" };
            
            return TypedResults.Ok(result);
        })
        .WithName("Logout")
        .Accepts<string>("application/json")
        .Produces<ApiResult<List<User>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithOpenApi();
    }
}
