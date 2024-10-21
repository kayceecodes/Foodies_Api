using System.Reflection;
using System.Text;
using foodies_api.Profiles;
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
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// temporarily hiding password in user-secrets
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
var dbPassword = configuration["DbPassword"];
conn += dbPassword;

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(conn));
builder.Services.AddAutoMapper(typeof(UserProfile), typeof(GetBusinessProfile), typeof(PostUserLikeBusinessProfile));

// builder.Services.AddDbContext<UserRolesContext>(opt => 
//     opt.UseInMemoryDatabase("ProductsDb")
// ); // For simplicity, using in-memory database

builder.Services.AddScoped<IUsersLikeBusinessesRepository, UsersLikeBusinessesRepository>();
builder.Services.AddScoped<IUsersLikeBusinessesService, UsersLikeBusinessesService>();
builder.Services.AddScoped<IBusinessService, BusinessService>();
builder.Services.AddScoped<IBusinessRepository, BusinessRepository>();
builder.Services.AddScoped<IFoodiesYelpService, FoodiesYelpService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// builder.Services.AddSwaggerGen(options =>
// {
//      options.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

//     // Add support for XML comments
//     var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//     var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//     options.IncludeXmlComments(xmlPath);
// });

// Configure services
builder.Services.AddHttpClient("FoodiesYelpService", client => 
{
    client.BaseAddress = new Uri(configuration.GetValue<string>("BaseAddress"));    
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = configuration["JwtSettings:Issuer"],
            ValidAudience = configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
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

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{   // Opens at http://localhost:(whatever port numer)/index.html
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
    options.RoutePrefix = string.Empty; // Set the Swagger UI at the root URL
});

// Order of app.func matters. Authentication before Authorization. Both Auths before Controllers/Endpointss
app.UseAuthentication();
app.UseAuthorization();

// app.UseHttpsRedirection();

app.ConfigurationUserEndpoints();
app.ConfigurationAuthEndpoints();
app.ConfigurationUserLikeBusinessEndpoints();

// app.UseCors();

app.Run();
