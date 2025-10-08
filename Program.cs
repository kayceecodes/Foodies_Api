using System.Text;
using foodies_api.Models.Mappings;
using foodies_api.Constants;
using foodies_api.Data;
using foodies_api.Endpoints;
using foodies_api.Interfaces.Repositories;
using foodies_api.Interfaces.Services;
using foodies_api.Repositories;
using foodies_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Cors.Infrastructure;
using foodies_api.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var AllowLocalDevelopment = "AllowLocalDevelopment";

var connectionString = configuration["ConnectionStrings:DefaultConnection"];
string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

if (environment == "Production") // Production in Docker read from files
{
    var secrets = await ContainerSecrets.ReadSecrets(configuration);
    builder.Configuration["JwtSettings:Key"] = secrets.JwtKey;
    connectionString = secrets.ConnectionString; 
}

var CorsPolicies = new Action<CorsPolicyBuilder>(policy =>
{
    policy.WithOrigins("http://localhost:3000", "https://localhost:3001")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
});

builder.Services.AddCors(options => options.AddPolicy(name: AllowLocalDevelopment, CorsPolicies));

builder.Services.AddDbContext<AppDbContext>(options => 
{

    options.UseNpgsql(connectionString);
    //options.EnableSensitiveDataLogging();
    //options.LogTo(Console.WriteLine, LogLevel.Information);
});

var serviceProvider = builder.Services.BuildServiceProvider();
var databaseHealthCheck = new DatabaseReadinessChecker<DevAppDbContext>(serviceProvider, 0, null);

if (environment == "Development")
    builder.Services.AddDbContext<DevAppDbContext>();

builder.Services.AddAutoMapper(typeof(PostUserProfile), typeof(GetBusinessProfile), typeof(PostUserLikeBusinessProfile));

builder.Services.AddScoped<IUsersLikeBusinessesRepository, UsersLikeBusinessesRepository>();
builder.Services.AddScoped<IUsersLikeBusinessesService, UsersLikeBusinessesService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IBusinessService, BusinessService>();
builder.Services.AddScoped<IBusinessRepository, BusinessRepository>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IFoodiesYelpService, FoodiesYelpService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});
builder.Services.AddHttpClient("FoodiesYelpService", client => client.BaseAddress = new Uri(configuration.GetValue<string>("BaseAddress")));

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = configuration["JwtSettings:Issuer"],
            ValidAudience = configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"])),
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        };

         // Read JWT from cookie instead of Authorization header
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["auth-token"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(Identity.AdminUserPolicyName, p =>
        p.RequireClaim(Identity.AdminUserClaimName, "true"));

var app = builder.Build();

await databaseHealthCheck.WaitUntilReadyAsync();

app.MapGet("/health", () => {
    Console.WriteLine("DIRECT: Health endpoint hit!");
    return Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
});

app.UseGlobalExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI(options =>
{   // Opens at http://localhost:(whatever port numer)/index.html
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
    options.RoutePrefix = string.Empty; // Set the Swagger UI at the root URL
});

app.UseCors(AllowLocalDevelopment);

// Order of app.func matters. Authentication before Authorization. Both Auths before Controllers/Endpointss
app.UseAuthentication();
app.UseAuthorization();

app.ConfigUserLikeEndpoints();
app.ConfigAuthEndpoints();
app.ConfigUserEndpoints();


app.Run();
