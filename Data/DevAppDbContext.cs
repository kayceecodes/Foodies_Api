using Microsoft.EntityFrameworkCore;
using foodies_api.Constants;

namespace foodies_api.Data;

/// <summary>
/// Represents the development database context, extending <see cref="AppDbContext"/>
/// and providing a factory method for instantiation to a database container.
/// Could not instantiate from app's container, so this class helps do it 
/// locally(local app to container DB).
/// </summary>
public class DevAppDbContext : AppDbContext
{
    public DevAppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Creates a new <see cref="DevAppDbContext"/> instance using the 
    /// provided configuration to build the connection string.
    /// </summary>
    /// <param name="config">The application configuration containing the "DbPassword" key.</param>
    /// <returns>A configured <see cref="DevAppDbContext"/> instance.</returns>
    public static DevAppDbContext Create(IConfiguration config)
    {
        var dbPassword = config["DbPassword"];
        var conn = $"User Id=postgres;Host=foodies-api-db;Port=5432;Database=foodiesapidb;Password={dbPassword};Pooling=true;";

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(conn);

        return new DevAppDbContext(optionsBuilder.Options);
    }
}