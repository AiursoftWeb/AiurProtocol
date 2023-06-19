using Aiursoft.AiurProtocol.Models;
using Aiursoft.AiurProtocol.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Aiursoft.AiurProtocol;

public static class Extensions
{
    public static IActionResult Protocol(this ControllerBase controller, ErrorType errorType, string errorMessage)
    {
        var logger = controller.HttpContext.RequestServices.GetRequiredService<ILogger<AiurResponse>>();
        return controller.Protocol(new AiurResponse
        {
            Code = errorType,
            Message = errorMessage
        });
    }

    public static IActionResult Protocol(this ControllerBase controller, AiurResponse model)
    {
        if (controller.HttpContext.Response.HasStarted)
        {
            return new EmptyResult();
        }

        controller.HttpContext.Response.StatusCode = (int)model.ConvertToHttpStatusCode();
        return new JsonResult(model);
    }
}
