using Microsoft.Extensions.DependencyInjection;

namespace FormBuilder.Core.Module;

public static class CoreModule
{
    public static void AddCoreModule(this IServiceCollection services)
    {
        services.AddScoped<IFormService, FormService>();
        
    }
}