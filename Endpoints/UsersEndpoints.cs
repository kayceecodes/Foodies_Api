using AutoMapper;
using foodies_api.Auth;
using foodies_api.Data;
using foodies_api.Models.Dtos;
using foodies_yelp.Models.Dtos.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace foodies_api.Endpoints;

public static class UserEndpoints
{
    public static void ConfigurationUserEndpoints(this WebApplication app) 
    {
        var api = app.MapGroup("/api");
        
        api.MapGet("/api/users", (ApplicationDbContext dbContext) =>
        {
            var users = dbContext.Users.ToList();

            return TypedResults.Ok(users);
        })
        .WithName("Get Users")
        .Accepts<string>("application/json")
        .Produces<APIResult<List<User>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        api.MapPost("/api/user", [Authorize] async ([FromServices] IMapper mapper, UserDto userDto, ApplicationDbContext dbContext) =>
        {

            var mappedUser = mapper.Map<GetUser>(userDto);
            var users = dbContext.Users.Add(mappedUser);

            await dbContext.SaveChangesAsync();
            return TypedResults.Ok(users);
        })
        .WithName("Add User")
        .Accepts<string>("application/json")
        .Produces<APIResult<List<User>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        api.MapPost("/login", async (User user, ApplicationDbContext dbContext, IConfiguration config) => 
        {
            var matchedUser = dbContext.Users.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
            var Auth = new Authentication(config);
            if(matchedUser == null)
                return Results.Empty;
            else
            {
                var token = Auth.CreateAccessToken(matchedUser);
               return TypedResults.Ok(token);
            }
        })
        .WithName("Login")
        .Accepts<string>("application/json")
        .Produces<APIResult<List<User>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithOpenApi();
    }
}
