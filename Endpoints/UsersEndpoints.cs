using AutoMapper;
using foodies_api.Auth;
using foodies_api.Data;
using foodies_api.Models.Dtos;
using foodies_api.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using foodies_api.Models;

namespace foodies_api.Endpoints;

public static class UserEndpoints
{
    public static void ConfigurationUserEndpoints(this WebApplication app) 
    {
        var appGroup = app.MapGroup("/api");
        
        appGroup.MapGet("/users", (AppDbContext dbContext) =>
        {
            var users = dbContext.Users.ToList();

            return TypedResults.Ok(users);
        })
        .WithName("Get Users")
        .Accepts<string>("application/json")
        .Produces<ApiResult<List<User>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithOpenApi();

        // appGroup.MapPost("/user", [Authorize(Policy = Identity.AdminUserPolicyName)] async ([FromServices] IMapper mapper, UserDto dto, AppContext db) =>
        appGroup.MapPost("/users/add", async ([FromServices] IMapper mapper, UserDto dto, AppDbContext db) =>
        {
            var mappedUser = mapper.Map<User>(dto);
            mappedUser.Id = Guid.NewGuid();
            var users = db.Users.Add(mappedUser);

            await db.SaveChangesAsync();
            
            return TypedResults.Ok(mappedUser);
        })
        .WithName("Add User")
        .Accepts<string>("application/json")
        .Produces<ApiResult<List<User>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithOpenApi();

        appGroup.MapPost("/login", async ([FromBody] UserDto dto, AppDbContext context, IConfiguration config, IMapper mapper) => 
        {
            var matchedUser = new User();
            if(!dto.Email.IsNullOrEmpty())
                matchedUser = context.Users.Where(u => u.Email == dto.Email && u.Password == dto.Password).FirstOrDefault();
            else 
                matchedUser = context.Users.Where(u => u.Username == dto.Username && u.Password == dto.Password).FirstOrDefault();
            
            var Auth = new Authentication(config);
            if(matchedUser == null)
                return Results.NotFound();
            
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

        appGroup.MapDelete("/users/delete/", async ([FromBody] UserDto userDto, AppDbContext context, IConfiguration config) => 
        {
            var user = await context.Users.FindAsync(userDto.Id);
            var result = new ApiResult<User>();
            
            if(user == null)
                return Results.NotFound();

            var users = context.Users.Remove(user);
            result = ApiResult<User>.Pass(data: user);
            
            await context.SaveChangesAsync();

            return TypedResults.Ok(result);

        })
        .WithName("Delete User")
        .Accepts<string>("application/json")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithOpenApi();
    }
}
