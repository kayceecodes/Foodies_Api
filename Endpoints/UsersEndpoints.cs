using foodies_api.Data;
using foodies_api.Models;
using Microsoft.AspNetCore.Mvc;
using foodies_api.Models.Entities;
using foodies_api.Models.Dtos.Requests;
using Microsoft.EntityFrameworkCore;
using System.Net;


namespace foodies_api.Endpoints;

public static class UserEndpoints
{
    public static void ConfigurationUserEndpoints(this WebApplication app) 
    {        
        app.MapGet("/api/users", (AppDbContext dbContext) =>
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

        app.MapDelete("/api/users/remove/", async ([FromBody] Guid userId, AppDbContext context, IConfiguration config) => 
        {
            var user = await context.Users.FindAsync(userId);
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

        app.MapPut("/api/users/edit/", async ([FromBody] UpdateUserRequest userRequest, AppDbContext context, IConfiguration config, HttpContext httpContext) => 
        {
            var userId = Guid.Parse(httpContext.User.FindFirst(userRequest.Username).Value);
            var rowsAffected = await context.Users.Where(u => u.Id == userId)
                .ExecuteUpdateAsync(updates => 
                    updates.SetProperty(u => u.Email, userRequest.Email)
                           .SetProperty(u => u.FirstAndLastName, userRequest.FirstName + " " + userRequest.LastName)
                           .SetProperty(u => u.Password, userRequest.Password)
                           .SetProperty(u => u.Username, userRequest.Username));
            
            var result = new ApiResult<UpdateUserRequest>()
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Data = userRequest,
            };

            return rowsAffected == 0 ? Results.NotFound() : TypedResults.Ok(result);
        })
        .WithName("Update User")
        .Accepts<string>("application/json")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi();
    }
}
