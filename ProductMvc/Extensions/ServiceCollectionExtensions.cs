using ProductMvc.Data;
using ProductMvc.Entities;
using Microsoft.EntityFrameworkCore;

namespace ProductMvc.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddAppDbContext(this IServiceCollection collection, ConfigurationManager configuration)
    {
        collection.AddDbContext<AppDbContext>(options =>
        {
            options.UseLazyLoadingProxies().UseSqlite("Data Source = productMvc.db");
        });
    }

    public static void AddIdentityManagers(this IServiceCollection collection)
    {
        collection.AddIdentity<User, UserRole>(options =>
        {
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
        }).AddRoles<UserRole>().AddEntityFrameworkStores<AppDbContext>();
    }
}
