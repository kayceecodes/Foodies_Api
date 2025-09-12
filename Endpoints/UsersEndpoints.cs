using foodies_api.Models;
using Microsoft.AspNetCore.Mvc;
using foodies_api.Models.Entities;
using foodies_api.Models.Dtos.Requests;
using foodies_api.Interfaces.Services;
using System.Security.Claims;

namespace foodies_api.Endpoints;

public static class UserEndpoints
{
    public static void ConfigUserEndpoints(this WebApplication app)
    {
        app.MapGet("/api/users", async Task<IResult> (IUsersService usersService) =>
        {
            var result = await usersService.GetAllUsers();

            if (!result.IsSuccess)
            {
                return TypedResults.BadRequest(result);
            }

            return TypedResults.Ok(result);
        })
        .WithName("Get Users")
        .Accepts<string>("application/json")
        .Produces<ApiResult<List<User>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithOpenApi();

        app.MapGet("/api/users/me", async Task<IResult> (HttpContext httpContext, IUsersService usersService) =>
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return TypedResults.Unauthorized();

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
                return TypedResults.BadRequest("Invalid user ID format");

            var result = await usersService.GetUserById(userId);

            if (!result.IsSuccess)
            {
                return TypedResults.NotFound(result.Message);
            }

            return TypedResults.Ok(result);
        })
        .RequireAuthorization() // This ensures the user must be authenticated
        .WithName("Get Current User")
        .Produces<ApiResult<User>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi();


        app.MapDelete("/api/users/{id}", async Task<IResult> (string id, IUsersService usersService) =>
        {
            Guid userId = Guid.Parse(id);
            var result = await usersService.DeleteUserById(userId);

            if (!result.IsSuccess)
            {
                return TypedResults.BadRequest(result);
            }

            return TypedResults.Ok(result);
        })
        .WithName("Delete User")
        .Accepts<string>("application/json")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi();

        app.MapPut("/api/users/{id}", async Task<IResult> (string id, [FromBody] UserUpdateRequest request, IUsersService usersService) =>
        {
            Guid userId = Guid.Parse(id);
            var result = await usersService.UpdateUser(userId, request);

            if (!result.IsSuccess)
            {
                return TypedResults.BadRequest(result);
            }

            return TypedResults.Ok(result);
        })
        .WithName("Update User")
        .Accepts<string>("application/json")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi();
    }
}
