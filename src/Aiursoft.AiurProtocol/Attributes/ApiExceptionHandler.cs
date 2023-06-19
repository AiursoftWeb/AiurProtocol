using System.Reflection;
using Aiursoft.AiurProtocol.Exceptions;
using Aiursoft.AiurProtocol.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aiursoft.AiurProtocol.Attributes;

/// <summary>
///     Adding this will handle `AiurAPIModelException` and return the result as JSON directly.
///     Adding this will handle `AiurUnexpectedResponse` and return the result as JSON directly.
/// </summary>
public class ApiExceptionHandler : ExceptionFilterAttribute
{
    public bool PassthroughRemoteErrors { get; set; } = true;
    
    public override void OnException(ExceptionContext context)
    {
        var projectName = Assembly.GetEntryAssembly()?.GetName().Name;

        base.OnException(context);
        switch (context.Exception)
        {
            // This exception happens on the server. When the server tries to access another AiurProtocol API and it returned unexpected response.
            case AiurUnexpectedServerResponseException exp:
                if (PassthroughRemoteErrors)
                {
                    context.ExceptionHandled = true;
                    context.Result = context.HttpContext.Protocol(exp.Response);
                }
                else
                {
                    var maskedResponse = new AiurResponse
                    {
                        Code = ErrorType.RemoteNotAccessible,
                        Message = $"The {projectName} server crashed with a remote API not accessible. Sorry about that."
                    };
                    context.ExceptionHandled = true;
                    context.Result = context.HttpContext.Protocol(maskedResponse);
                }
                break;
            case AiurServerException exp:
                context.ExceptionHandled = true;
                context.Result = context.HttpContext.Protocol(exp.Response);
                break;
            default:
                var response = new AiurResponse
                {
                    Code = ErrorType.UnknownError,
                    Message = $"The {projectName} server crashed with an unknown error. Sorry about that."
                };
                context.ExceptionHandled = true;
                context.Result = context.HttpContext.Protocol(response);
                break;
        }
    }
}