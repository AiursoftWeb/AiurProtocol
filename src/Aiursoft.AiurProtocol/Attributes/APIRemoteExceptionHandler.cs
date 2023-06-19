using Aiursoft.AiurProtocol.Exceptions;
using Aiursoft.AiurProtocol.Models;
using Aiursoft.AiurProtocol.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aiursoft.AiurProtocol.Attributes;

/// <summary>
///     Adding this will handle `AiurAPIModelException` and return the result as JSON directly.
///     Adding this will handle `AiurUnexpectedResponse` and return the result as JSON directly.
/// </summary>
public class APIRemoteExceptionAiurProtocol : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        base.OnException(context);
        switch (context.Exception)
        {
            case AiurUnexpectedResponse exp:
                context.ExceptionHandled = true;
                context.HttpContext.Response.StatusCode = (int)exp.Response.ConvertToHttpStatusCode();
                context.Result = new JsonResult(new AiurResponse { Code = exp.Code, Message = exp.Message });
                break;
            case AiurAPIModelException exp:
                context.ExceptionHandled = true;
                context.HttpContext.Response.StatusCode =
                    (int)new AiurResponse { Code = exp.Code }.ConvertToHttpStatusCode();
                context.Result = new JsonResult(new AiurResponse { Code = exp.Code, Message = exp.Message });
                break;
        }
    }
}