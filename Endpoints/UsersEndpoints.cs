using AutoMapper;
using foodies_api.Auth;
using foodies_api.Data;
using foodies_api.Models.Dtos;
using foodies_api.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Tokens;

namespace foodies_api.Endpoints;

public static class UserEndpoints
{
    public static void ConfigurationUserEndpoints(this WebApplication app) 
    {
        var appGroup = app.MapGroup("/api");
        
        appGroup.MapGet("/users", (ApplicationDbContext dbContext) =>
        {
            var users = dbContext.Users.ToList();

            return TypedResults.Ok(users);
        })
        .WithName("Get Users")
        .Accepts<string>("application/json")
        .Produces<ApiResult<List<User>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        // appGroup.MapPost("/user", [Authorize(Policy = Identity.AdminUserPolicyName)] async ([FromServices] IMapper mapper, UserDto dto, ApplicationDbContext db) =>
        appGroup.MapPost("/adduser", async ([FromServices] IMapper mapper, UserDto dto, ApplicationDbContext db) =>
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
        .Produces(StatusCodes.Status500InternalServerError);

        appGroup.MapPost("/login", async ([FromBody] UserDto dto, ApplicationDbContext db, IConfiguration config) => 
        {
            var matchedUser = new User();
            if(!dto.Email.IsNullOrEmpty())
                matchedUser = db.Users.Where(u => u.Email == dto.Email && u.Password == dto.Password).FirstOrDefault();
            else 
                matchedUser = db.Users.Where(u => u.UserName == dto.Username && u.Password == dto.Password).FirstOrDefault();
            
            var Auth = new Authentication(config);
            if(matchedUser == null)
                return Results.Empty;
            
            var token = Auth.CreateAccessToken(matchedUser);
            
            return TypedResults.Ok(token);
        })
        .WithName("Login")
        .Accepts<string>("application/json")
        .Produces<ApiResult<List<User>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithOpenApi();

         appGroup.MapPost("/user/delete", async (Guid userId, ApplicationDbContext db, IConfiguration config) => 
        {
            var matchedUser = db.Users.Where(u => u.Id == userId).FirstOrDefault();
            var user = db.Users.Remove(matchedUser);
            var 
            ApiResult<User> response = user;

             
            return TypedResults.Ok(matchedUser);
        })
        .WithName("Delete User")
        .Accepts<string>("application/json")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithOpenApi();
    }
}
