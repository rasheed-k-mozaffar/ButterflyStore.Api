using System.Text;
using ButterflyStore.Server.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ButterflyStore.Server.Extensions;

public static class ServiceExtensions
{
    //Configuring the application db context to use SQL Lite and setting up the connection string called
    //Default Connection from the appsettings.development.json.
    public static void AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });
    }

    //Add AutoMapper to the DI container.
    public static void RegisterAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }

    //Add the IProductsService to the DI container.
    public static void AddProductsService(this IServiceCollection services)
    {
        services.AddScoped<IProductsService, ProductsService>();
    }

    //Add the ICategoryService to the DI container.
    public static void AddCategoryService(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
    }

    //Add and Configure Identity.
    public static void AddAndConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, IdentityRole>(o =>
        {
            //Configure the password
            o.Password.RequireDigit = true;
            o.Password.RequireUppercase = true;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequiredLength = 8;
        }).AddDefaultTokenProviders()
          .AddEntityFrameworkStores<AppDbContext>();
    }
    

}