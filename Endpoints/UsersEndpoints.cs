using foodies_api.Data;
using foodies_api.Models.Dtos;
using foodies_api.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using foodies_api.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

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

        appGroup.MapPut("/users/update/", async ([FromBody] UserDto userDto, AppDbContext context, IConfiguration config) => 
        {
            var rowsAffected = await context.Users.Where(u => u.Id == userDto.Id)
                .ExecuteUpdateAsync(updates => 
                    updates.SetProperty(u => u.Email, userDto.Email)
                           .SetProperty(u => u.FirstAndLastName, userDto.FirstName + " " + userDto.LastName)
                           .SetProperty(u => u.Password, userDto.Password)
                           .SetProperty(u => u.Username, userDto.Username));
            
            var result = new ApiResult<UserDto>()
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Data = userDto,
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
