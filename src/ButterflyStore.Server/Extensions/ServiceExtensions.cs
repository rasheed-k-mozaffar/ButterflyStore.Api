using ButterflyStore.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace ButterflyStore.Server.Extensions;

public static class ServiceExtensions
{
    public static void AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });
    }
}