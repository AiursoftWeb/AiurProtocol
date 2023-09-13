using Aiursoft.AiurProtocol.Server;
using Aiursoft.WebTools.Models;
using Microsoft.AspNetCore.HttpOverrides;
using static Aiursoft.WebTools.Extends;

namespace DemoApiApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        var app = App<Startup>(args);
        await app.RunAsync();
    }
}

public class Startup : IWebStartup
{
    public void ConfigureServices(IConfiguration configuration, IWebHostEnvironment environment, IServiceCollection services)
    {
        services
            .AddControllers()
            .AddApplicationPart(typeof(Startup).Assembly)
            .AddAiurProtocol();
    }

    public void Configure(WebApplication app)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
        app.UseHttpsRedirection();
        app.UseRouting();
        app.MapDefaultControllerRoute();
    }
}