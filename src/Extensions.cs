using Microsoft.EntityFrameworkCore;

namespace PersonMinimalApi;

public static class Extensions
{
    internal static void RegisterConnectionString(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(x => x.UseNpgsql(configuration.GetConnectionString("Default")));
    }
}