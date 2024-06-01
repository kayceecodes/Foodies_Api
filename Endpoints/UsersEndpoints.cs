
using AutoMapper;
using foodies_api.Data;
using foodies_api.Models.Dtos;
using foodies_yelp.Models.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;

public static class UserEndpoints
{
    public static void ConfigurationUserEndpoints(this WebApplication app) 
    {
        app.MapGet("/api/user", async Task<IResult> (ApplicationDbContext dbContext) =>
        {
            var users = dbContext.Users.ToList();
            
            return TypedResults.Ok(users);
        }).WithName("GetUserByNameAndLocation").Accepts<string>("application/json")
        .Produces<APIResult<List<User>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapPost("/users", async (User user, ApplicationDbContext dbContext) =>
        {
            var users = dbContext.Users.Add(user);

            await dbContext.SaveChangesAsync();
            return Results.Ok(users);
        });
    }
}
