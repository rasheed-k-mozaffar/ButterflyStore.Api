using ButterflyStore.Server.Data;
using Microsoft.EntityFrameworkCore;

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

    public static void RegisterAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}