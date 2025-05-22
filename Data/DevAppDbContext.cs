using Microsoft.EntityFrameworkCore;
using foodies_api.Constants;

namespace foodies_api.Data;
public class DevAppDbContext : AppDbContext
{
    public DevAppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public static DevAppDbContext Create(IConfiguration config)
    {
        var dbPassword = config["DbPassword"];
        var conn = $"User Id=postgres;Host=foodies-api-db;Port=5432;Database=foodiesapidb;Password={dbPassword};Pooling=true;";

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(conn);

        return new DevAppDbContext(optionsBuilder.Options);
    }
}