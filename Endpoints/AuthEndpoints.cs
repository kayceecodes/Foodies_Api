using Microsoft.AspNetCore.Mvc;
using foodies_api.Models.Dtos.Requests;
using foodies_api.Models;
using foodies_api.Models.Entities;
using foodies_api.Interfaces.Services;
using foodies_api.Models.Dtos.Responses;
using System.Security.Claims;
using System.Net;
using Microsoft.IdentityModel.Tokens;

namespace foodies_api.Endpoints;

public static class AuthEndpoints
{
    public static void ConfigAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/api/auth/login", async Task<IResult>
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

        app.MapPost("/api/auth/register", async Task<IResult>
        ([FromBody] RegisterRequest request, IAuthService service, HttpContext httpContext) =>
        {
            var result = await service.Register(request);

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
        .WithName("Register")
        .Accepts<string>("application/json")
        .Produces<ApiResult<List<User>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithOpenApi();

        app.MapPost("/api/auth/logout", async Task<IResult>
        ([FromBody] IAuthService service, HttpContext context) =>
        {
            var wasLoggedIn = context.User.Identity?.IsAuthenticated ?? false;

            // Delete the cookie - try multiple approaches to ensure it's gone
            context.Response.Cookies.Delete("auth-token", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/"
            });


            var result = new ApiResult<object>()
            {
                IsSuccess = true,
                Message = wasLoggedIn ? "Logged out successfully" : "User was not logged in",
                StatusCode = (HttpStatusCode)StatusCodes.Status200OK
            };
            Console.WriteLine($"User logged out. IsAuthenticated: {context.User.Identity.IsAuthenticated}");
            return TypedResults.Ok(result);
        })
        .WithName("Logout")
        .Accepts<string>("application/json")
        .Produces<ApiResult<List<User>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithOpenApi();

        app.MapGet("/api/auth/verify", async Task<IResult>(HttpContext context) =>
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = context.User.FindFirst(ClaimTypes.Name)?.Value;

            if (userId.IsNullOrEmpty() && email.IsNullOrEmpty())
            {
                Console.WriteLine("UserId and/or Email has no value");
                return TypedResults.NotFound(new ApiResult<object>
                {
                    IsSuccess = false,
                    ErrorMessages = ["No users are logged in"]
                });
            }

            Console.WriteLine("User Verified");
            return TypedResults.Ok(new ApiResult<object>
            {
                IsSuccess = true,
                Data = new
                {
                    isAuthenticated = true,
                    email,
                    userId
                }
            });
        })
        .WithName("Verify Authentication")
        .WithOpenApi();
    }
}
