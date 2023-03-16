using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductMvc.Entities;
using System.Reflection;

namespace ProductMvc.Data;

public class AppDbContext : IdentityDbContext<User, UserRole, int>
{
    public DbSet<Product> Products { get; set; }
    public DbSet<History> Histories { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}