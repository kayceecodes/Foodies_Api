using System.Text.Json;

namespace foodies_api.Utils;
public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    error = "An unexpected error occurred.",
#if DEBUG
                    details = ex.Message
#else
                    details = "Please contact support."
#endif
                };

                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
        });
    }
}