using FormBuilder.Core.Database;
using FormBuilder.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FormBuilder.Core.Module;

public static class CoreModule
{
    public static void AddCoreModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FormContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                x => x.MigrationsAssembly("FormBuilder.Core")));

        services.AddScoped<IFormService, FormService>();
        services.AddScoped<IFormRepository, FormRepository>();
    }
}