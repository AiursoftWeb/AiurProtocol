using Aiursoft.AiurProtocol.Server;
using Aiursoft.WebTools;
using Microsoft.AspNetCore.HttpOverrides;

namespace DemoApiApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        var app = Extends.App<Startup>(args);
        await app.RunAsync();
    }
}

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddControllers()
            .AddApplicationPart(typeof(Startup).Assembly)
            .AddAiurProtocol();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
    }
}