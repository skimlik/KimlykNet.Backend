using KimlykNet.Data;

using Microsoft.EntityFrameworkCore;

namespace KimlykNet.Backend.Infrastructure.Database;

internal static class DataAccessConfigurationExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        
        var appConnectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(appConnectionString, b => b.MigrationsAssembly("KimlykNet.Backend"));
        });

        return services;
    }
}