using Aiursoft.AiurProtocol.Services;
using Aiursoft.Canon;
using Microsoft.Extensions.DependencyInjection;

namespace Aiursoft.AiurProtocol;

public static class Extensions
{
    /// <summary>
    /// Add AiurProtocol Api Client to your SDK so your wrapper can use it to call your real APIs.
    ///
    ///  (If your project is using Aiursoft.Scanner, you do NOT have to call this!)
    /// </summary>
    /// <param name="services"></param>
    /// <param name="addHttpClient">If it should also add an HttpClient to services.</param>
    /// <param name="addMemoryCache">If it should also add an MemoryCache to services.</param>
    /// <returns></returns>
    public static IServiceCollection AddAiurProtocolClient(
        this IServiceCollection services, 
        bool addHttpClient = true,
        bool addMemoryCache = true)
    {
        if (addHttpClient)
        {
            services.AddHttpClient();
        }
        services.AddTaskCanon(addMemoryCache: addMemoryCache);
        services.AddScoped<AiurProtocolClient>();
        return services;
    }
}
