﻿using System.Reflection;
using Aiursoft.AiurProtocol.Exceptions;
using Aiursoft.AiurProtocol.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Aiursoft.AiurProtocol.Server.Attributes;

/// <summary>
///     Adding this will handle `AiurAPIModelException` and return the result as JSON directly.
///     Adding this will handle `AiurUnexpectedResponse` and return the result as JSON directly.
/// </summary>
public class ApiExceptionHandler : ExceptionFilterAttribute
{
    public bool PassthroughRemoteErrors { get; set; } = true;

    public bool PassthroughAiurServerException { get; set; } = true;
    
    public bool SuppressExceptionFromLog { get; set; } = false;
    
    public override void OnException(ExceptionContext context)
    {
        var projectName = Assembly.GetEntryAssembly()?.GetName().Name;
        
        if (!SuppressExceptionFromLog)
        {
            var logger = context.HttpContext.RequestServices.GetService(typeof(ILogger<ApiExceptionHandler>)) as ILogger<ApiExceptionHandler>;
            logger?.LogCritical(context.Exception, "The {projectName} server crashed with an exception.", projectName);
        }
        
        base.OnException(context);
        switch (context.Exception)
        {
            // This exception happens on the server. When the server tries to access another AiurProtocol API and it returned unexpected response.
            case AiurUnexpectedServerResponseException exp:
                if (PassthroughRemoteErrors)
                {
                    ProcessResult(context, exp.Response);
                    return;
                }
                else
                {
                    var maskedResponse = new AiurResponse
                    {
                        Code = Code.RemoteNotAccessible,
                        Message = $"The {projectName} server crashed with a remote API not accessible. Sorry about that."
                    };
                    ProcessResult(context, maskedResponse);
                    return;
                }
            
            // Known server exception. Terminate the program.
            case AiurServerException exp:
                if (PassthroughAiurServerException)
                {
                    ProcessResult(context, exp.Response);
                    return;
                }
                else
                {
                    var maskedResponse = new AiurResponse
                    {
                        Code = Code.UnknownError,
                        Message = $"The {projectName} server had an internal error that it couldn't process your request."
                    };
                    ProcessResult(context, maskedResponse);
                    return;
                }
            
            // Unknown error.
            default:
                var response = new AiurResponse
                {
                    Code = Code.UnknownError,
                    Message = $"The {projectName} server crashed with an unknown error. Sorry about that."
                };
                ProcessResult(context, response);
                return;
        }
    }

    private void ProcessResult(ExceptionContext context, AiurResponse response)
    {
        if (SuppressExceptionFromLog)
        {
            context.ExceptionHandled = true;
        }
        context.Result = context.HttpContext.Protocol(response);
    }
}