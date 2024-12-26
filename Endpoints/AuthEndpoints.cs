using AutoMapper;
using foodies_api.Auth;
using foodies_api.Data;
using foodies_api.Models.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using foodies_api.Models;
using System.Net;
using foodies_api.Models.Entities;

namespace foodies_api.Endpoints;

public static class AuthEndpoints
{
    public static void ConfigurationAuthEndpoints(this WebApplication app)
    {
        var appGroup = app.MapGroup("/api/auth");

        // appGroup.MapPost("/user", [Authorize(Policy = Identity.AdminUserPolicyName)] async ([FromServices] IMapper mapper, UserDto dto, AppContext db) =>
        appGroup.MapPost("/login", async ([FromBody] UserDto dto, AppDbContext context, IConfiguration config, IMapper mapper) =>
        {
            var matchedUser = new User();
            if (!dto.Email.IsNullOrEmpty())
                matchedUser = context.Users.Where(u => u.Email == dto.Email && u.Password == dto.Password).FirstOrDefault();
            else
                matchedUser = context.Users.Where(u => u.Username == dto.Username && u.Password == dto.Password).FirstOrDefault();

            var Auth = new Authentication(config);
            if (matchedUser == null)
                return Results.NotFound("User not found.");

            var token = Auth.CreateAccessToken(matchedUser);
            var mappedUserDto = mapper.Map<UserDto>(matchedUser);
            mappedUserDto.Token = token;

            var result = ApiResult<UserDto>.Pass(mappedUserDto);

            return TypedResults.Ok(result);
        })
        .WithName("Login")
        .Accepts<string>("application/json")
        .Produces<ApiResult<List<User>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithOpenApi();

        appGroup.MapPost("/register", async ([FromBody] RegisterDto dto, AppDbContext context, IConfiguration config, IMapper mapper) =>
        {
            var result = new ApiResult<RegisterDto>();
            bool usernameexists = context.Users.Any(user => user.Username == dto.Username);
            bool emailexists = context.Users.Any(user => user.Email == dto.Email);

            if (usernameexists)
                return Results.Conflict("Username already exists");

            if (emailexists)
            {
                result = new ApiResult<RegisterDto>()
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.Accepted,
                    ErrorMessages = ["Email already exists, use another email or recover your account"]
                };
                return TypedResults.Ok(result);
            }

            var mappedUser = mapper.Map<User>(dto);
            mappedUser.Id = Guid.NewGuid();
            context.Users.Add(mappedUser);

            await context.SaveChangesAsync();

            var Auth = new Authentication(config);

            var token = Auth.CreateAccessToken(mappedUser);
            var mappedUserDto = mapper.Map<UserDto>(mappedUser);
            mappedUserDto.Token = token;

            ApiResult<UserDto> response = ApiResult<UserDto>.Pass(mappedUserDto);

            return TypedResults.Ok(response);
        })
        .WithName("Register User")
        .Accepts<string>("application/json")
        .Produces<ApiResult<List<User>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithOpenApi();
    }
}
