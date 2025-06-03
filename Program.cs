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


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var AllowLocalDevelopment = "AllowLocalDevelopment";

var conn = configuration["ConnectionStrings:DefaultConnection"];
string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

// Production in Docker container
// Helper to ensure env var exists
string GetRequiredEnv(string name)
{
    var value = Environment.GetEnvironmentVariable(name);
    if (string.IsNullOrWhiteSpace(value))
        throw new InvalidOperationException($"Environment variable '{name}' is not set.");
    return value;
}

// Helper to ensure secret file exists and is readable
string GetRequiredSecret(string path, string name)
{
    if (!File.Exists(path))
        throw new FileNotFoundException($"Required secret '{name}' not found at path '{path}'.");
    var value = File.ReadAllText(path).Trim();
    if (string.IsNullOrWhiteSpace(value))
        throw new InvalidOperationException($"Secret '{name}' in file '{path}' is empty or invalid.");
    return value;
}

if (environment == "Production")
{
    var jwtKey = GetRequiredSecret("/run/secrets/jwt-secret", "JwtSettings:Key");
    var dbHost = GetRequiredEnv("DB_HOST");
    var dbPort = GetRequiredEnv("DB_PORT");
    var dbUser = GetRequiredEnv("DB_USER");
    var dbName = GetRequiredEnv("DB_NAME");
    var dbPassword = GetRequiredSecret("/run/secrets/postgres-passwd", "Database Password");

    conn = $"Host={dbHost};Port={dbPort};User ID={dbUser};Password={dbPassword};Database={dbName};Pooling=true;";
    builder.Configuration["ConnectionStrings:DefaultConnection"] = conn;
    builder.Configuration["JwtSettings:Key"] = jwtKey;
}

var CorsPolicies = new Action<CorsPolicyBuilder>(policy =>
{
    policy.WithOrigins("http://localhost:3000",
                        "http://localhost:3001")
          .AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod();
});

builder.Services.AddCors(options => options.AddPolicy(name: AllowLocalDevelopment, CorsPolicies));

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(conn));
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
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Configure Http Client to Foodies Yelp Service
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
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Identity.AdminUserPolicyName, p =>
        p.RequireClaim(Identity.AdminUserClaimName, "true"));
});

var app = builder.Build();

app.UseGlobalExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI(options =>
{   // Opens at http://localhost:(whatever port numer)/index.html
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
    options.RoutePrefix = string.Empty; // Set the Swagger UI at the root URL
});

// Order of app.func matters. Authentication before Authorization. Both Auths before Controllers/Endpointss
app.UseAuthentication();
app.UseAuthorization();

app.ConfigUserLikeEndpoints();
app.ConfigAuthEndpoints();
app.ConfigUserEndpoints();

app.UseCors(AllowLocalDevelopment);

app.Run();
