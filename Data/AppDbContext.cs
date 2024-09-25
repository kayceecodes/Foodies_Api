using Microsoft.EntityFrameworkCore;
using foodies_api.Models;
using System.Collections.Generic;

namespace foodies_api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<User> Users { get; set; }
    public DbSet<Business> Businesses { get; set; }
    public DbSet<UserLikeBusiness> UserLikeBusinesses { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .HasKey(user => user.Id);

        builder.Entity<Business>()
            .HasKey(business => business.Id);

        builder.Entity<UserLikeBusiness>().HasKey(ub => new { ub.UserId, ub.BusinessId });

        builder.Entity<UserLikeBusiness>()
            .HasOne<Business>(ub => ub.Business)
            .WithMany(u => u.UserLikeBusinesses);

        builder.Entity<UserLikeBusiness>()
            .HasOne<User>(ub => ub.User)
            .WithMany(u => u.UserLikeBusinesses);

        base.OnModelCreating(builder);
    }
}
