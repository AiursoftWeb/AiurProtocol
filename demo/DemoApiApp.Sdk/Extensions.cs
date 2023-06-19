using Aiursoft.AiurProtocol;
using Microsoft.Extensions.DependencyInjection;

namespace DemoApiApp.Sdk;

public static class Extensions
{
    public static IServiceCollection AddDemoService(this IServiceCollection services, string endPointUrl)
    {
        services.AddAiurApiClient();
        services.Configure<DemoServerConfig>(options => options.Instance = endPointUrl);
        services.AddScoped<DemoAccess>();
        return services;
    }
}
