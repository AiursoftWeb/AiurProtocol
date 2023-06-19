﻿#pragma warning disable CS0618 // Type or member is obsolete
using Aiursoft.AiurProtocol.Interfaces;
using Aiursoft.AiurProtocol.Models;
using Aiursoft.AiurProtocol.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Aiursoft.AiurProtocol;

public static class Extensions
{
    public static async Task<IActionResult> Protocol<T>(this ControllerBase controller, ErrorType errorType, string errorMessage, IOrderedQueryable<T> query, IPageable pager)
    {
        return controller.Protocol(await AiurPagedCollectionBuilder.BuildAsync(query, pager, errorType, errorMessage));
    }

    public static IActionResult Protocol<T>(this ControllerBase controller, ErrorType errorType, string errorMessage, IReadOnlyCollection<T> value)
    {
        return controller.Protocol(new AiurCollection<T>(value)
        {
            Code = errorType,
            Message = errorMessage
        });
    }

    public static IActionResult Protocol<T>(this ControllerBase controller, ErrorType errorType, string errorMessage, T value)
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

    [Obsolete(error: false, message: "Please pass the error type and error message directly!")]
    public static IActionResult Protocol(this ControllerBase controller, AiurResponse model)
    {
        var logger = controller.HttpContext.RequestServices.GetRequiredService<ILogger<AiurResponse>>();

        var logLevel = model.ConvertToLogLevel();
        logger.Log(logLevel, 0, null, "An API generated response with error code: {CODE} and message: '{MESSAGE}'", model.Code, model.Message ?? string.Empty);

        if (controller.HttpContext.Response.HasStarted)
        {
            return new EmptyResult();
        }

        controller.HttpContext.Response.StatusCode = (int)model.ConvertToHttpStatusCode();
        return new JsonResult(model);
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
