using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Aiursoft.AiurProtocol.Server;

public static class Extensions
{
    public static IMvcBuilder AddAiurProtocol(this IMvcBuilder services)
    {
        JsonConvert.DefaultSettings = () => ProtocolSettings.JsonSettings;
        return services.AddNewtonsoftJson(SetupAction);
    }
    
    public static Action<MvcNewtonsoftJsonOptions> SetupAction => options =>
    {
        options.SerializerSettings.DateTimeZoneHandling = ProtocolSettings.JsonSettings.DateTimeZoneHandling;
        options.SerializerSettings.ContractResolver = ProtocolSettings.JsonSettings.ContractResolver;
    };

    public static async Task<IActionResult> Protocol<T>(this ControllerBase controller, Code code, string errorMessage, IOrderedQueryable<T> query, Pager pager)
    {
        return controller.Protocol(await AiurPagedCollectionBuilder.BuildAsync(query, pager, code, errorMessage));
    }

    public static IActionResult Protocol<T>(this ControllerBase controller, Code code, string errorMessage, IReadOnlyCollection<T> items)
    {
        return controller.Protocol(new AiurCollection<T>(items)
        {
            Code = code,
            Message = errorMessage
        });
    }

    public static IActionResult Protocol<T>(this ControllerBase controller, Code code, string errorMessage, T value) where T : struct
    {
        return controller.Protocol(new AiurValue<T>(value)
        {
            Code = code,
            Message = errorMessage
        });
    }

    public static IActionResult Protocol(this ControllerBase controller, Code code, string errorMessage)
    {
        return controller.Protocol(new AiurResponse
        {
            Code = code,
            Message = errorMessage
        });
    }

    public static IActionResult Protocol(this ControllerBase controller, AiurResponse model)
    {
        return controller.HttpContext.Protocol(model);
    }
    
    public static IActionResult Protocol(this ControllerBase controller, AiurRelativePath relativePath)
    {
        return controller.Redirect(relativePath.ToString());
    }
    
    public static IActionResult Protocol(this HttpContext context, AiurResponse model)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<AiurResponse>>();

        var logLevel = model.ConvertToLogLevel();
        logger.Log(logLevel, 0, null, "An API generated response with error code: {Code} and message: '{Message}'", model.Code, model.Message ?? string.Empty);

        if (context.Response.HasStarted)
        {
            logger.LogCritical("Failed to generate AiurProtocol response because the response was already started");
            return new EmptyResult();
        }

        context.Response.StatusCode = (int)model.ConvertToHttpStatusCode();
        return new JsonResult(model);
    }
}