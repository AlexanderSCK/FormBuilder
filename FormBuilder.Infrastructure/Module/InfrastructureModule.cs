using FormBuilder.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using FormBuilder.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FormBuilder.Infrastructure.Module;

public static class InfrastructureModule
{
    public static void AddInfrastructureModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FormContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                x => x.MigrationsAssembly("FormBuilder.Infrastructure")));
        services.AddScoped<IFormRepository, FormRepository>();
    }
}