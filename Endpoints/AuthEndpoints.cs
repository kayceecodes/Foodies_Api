using AutoMapper;
using foodies_api.Auth;
using foodies_api.Data;
using foodies_api.Models.Dtos.Requests;
using foodies_api.Models.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using foodies_api.Models;
using System.Net;
using foodies_api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using foodies_api.Interfaces.Services;

namespace foodies_api.Endpoints;

public static class AuthEndpoints
{
    public static void ConfigurationAuthEndpoints(this WebApplication app)
    {
        // app.MapPost("/user", [Authorize(Policy = Identity.AdminUserPolicyName)] async ([FromServices] IMapper mapper, UserDto dto, AppContext db) =>
        app.MapPost("/api/auth/login", async Task<IResult>
        ([FromBody] LoginRequest request, IAuthService service) =>
        {
            var result = await service.Login(request);

            if (!result.IsSuccess)
                return TypedResults.BadRequest(result);

            return TypedResults.BadRequest(result);
            //var matchedUser = new User();
            //if (!request.Email.IsNullOrEmpty())
                //matchedUser = await context.Users.Where(u => u.Email == request.Email && u.Password == request.Password).FirstOrDefaultAsync();
            //else
                //matchedUser = await context.Users.Where(u => u.Username == request.Username && u.Password == request.Password).FirstOrDefaultAsync();

            //var Auth = new Authentication(config);
            //if (matchedUser == null)
                //return Results.NotFound("User not found.");

            //var token = Auth.CreateAccessToken(matchedUser);
            //var LoginResponse = mapper.Map<LoginResponse>(matchedUser);
            //LoginResponse.Token = token;

            //var result = ApiResult<LoginResponse>.Pass(LoginResponse);

            //return TypedResults.Ok(result);
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
            //var result = new ApiResult<RegisterRequest>();
            //bool usernameexists = await context.Users.AnyAsync(user => user.Username == request.Username);
            //bool emailexists = await context.Users.AnyAsync(user => user.Email == request.Email);

            //if (usernameexists)
            //return Results.Conflict("Username already exists");

            //if (emailexists)
            //{
            //result = new ApiResult<RegisterRequest>()
            //{
            //IsSuccess = false,
            //StatusCode = HttpStatusCode.Accepted,
            //ErrorMessages = ["Email already exists, use another email"]
            //};
            //return TypedResults.Ok(result);
            //}

            //var mappedUser = mapper.Map<User>(request);
            //mappedUser.Id = Guid.NewGuid();
            //context.Users.Add(mappedUser);

            //await context.SaveChangesAsync();

            //var Auth = new Authentication(config);

            //var token = Auth.CreateAccessToken(mappedUser);
            //var mappedResponse = mapper.Map<RegisterResponse>(request);
            //mappedResponse.Token = token;

            //ApiResult<RegisterResponse> response = ApiResult<RegisterResponse>.Pass(mappedResponse);

            //return TypedResults.Ok(response);
        })
        .WithName("Register User")
        .Accepts<string>("application/json")
        .Produces<ApiResult<List<User>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithOpenApi();
    }
}
