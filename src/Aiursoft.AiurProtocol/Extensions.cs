using Aiursoft.AiurProtocol.Interfaces;
using Aiursoft.AiurProtocol.Models;
using Aiursoft.AiurProtocol.Services;
using Aiursoft.Canon;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Aiursoft.AiurProtocol;

public static class Extensions
{
    public static IServiceCollection AddAiurApiClient(this IServiceCollection services)
    {
        services.AddTaskCanon();
        services.AddHttpClient();
        services.AddScoped<AiurApiClient>();
        return services;
    }
    
    public static async Task<IActionResult> Protocol<T>(this ControllerBase controller, ErrorType errorType, string errorMessage, IOrderedQueryable<T> query, IPager pager)
    {
        return controller.Protocol(await AiurPagedCollectionBuilder.BuildAsync(query, pager, errorType, errorMessage));
    }

    public static IActionResult Protocol<T>(this ControllerBase controller, ErrorType errorType, string errorMessage, IReadOnlyCollection<T> items)
    {
        return controller.Protocol(new AiurCollection<T>(items)
        {
            Code = errorType,
            Message = errorMessage
        });
    }

    public static IActionResult Protocol<T>(this ControllerBase controller, ErrorType errorType, string errorMessage, T value) where T : struct
    {
        return controller.Protocol(new AiurValue<T>(value)
        {
            Code = errorType,
            Message = errorMessage
        });
    }

    public static IActionResult Protocol(this ControllerBase controller, ErrorType errorType, string errorMessage)
    {
        return controller.Protocol(new AiurResponse
        {
            Code = errorType,
            Message = errorMessage
        });
    }

    public static IActionResult Protocol(this ControllerBase controller, AiurResponse model)
    {
        return controller.HttpContext.Protocol(model);
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
