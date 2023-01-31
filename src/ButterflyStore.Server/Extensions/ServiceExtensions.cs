using System.Text;
using ButterflyStore.Server.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ButterflyStore.Server.Extensions;

public static class ServiceExtensions
{
    /// <summary>
    /// This method registers the App Db Context to the DI container and reads the connection
    /// string from appsettings.development.json to establish a connection with the database.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
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

    //Add the IAuthService to the DI container.
    public static void AddAuthService(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
    }


    /// <summary>
    /// This method adds Identity and Configures the password settings for the needs of this project.
    /// </summary>
    /// <param name="services"></param>
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

    
    /// <summary>
    /// This methods add authentication and configures the token settings.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration">Used to read the secret key from settings store.</param>
    public static void AddAndConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            var secret = configuration["JwtSettings:Secret"];
            o.TokenValidationParameters = new TokenValidationParameters
            {
                //Configuring the JWT Token parameters and creating the Signing Key.
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!)),
                RequireExpirationTime = true
            };
        });
    }

    public static void AddApiVersioningService(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            //This option provides the API versions we have to the client;
            options.ReportApiVersions = true;

            //This will provide a default API version in case we didn't specify one.
            options.AssumeDefaultVersionWhenUnspecified = true;

            options.DefaultApiVersion = Microsoft.AspNetCore.Mvc.ApiVersion.Default;
        });
    }


}