﻿using Aiursoft.AiurProtocol.Models;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Aiursoft.AiurProtocol.Services;

internal static class StatusCodeTranslator
{
    public static HttpStatusCode ConvertToHttpStatusCode(this AiurResponse response)
    {
        return response.Code switch
        {
            Code.Success => HttpStatusCode.OK,
            Code.WrongKey => HttpStatusCode.Unauthorized,
            Code.PlaceHolder2 => HttpStatusCode.Gone,
            Code.PlaceHolder3 => HttpStatusCode.Gone,
            Code.NotFound => HttpStatusCode.NotFound,
            Code.UnknownError => HttpStatusCode.InternalServerError,
            Code.RemoteNotAccessible => HttpStatusCode.InternalServerError,
            Code.Conflict => HttpStatusCode.Conflict,
            Code.Unauthorized => HttpStatusCode.Unauthorized,
            Code.Timeout => HttpStatusCode.RequestTimeout,
            Code.InvalidInput => HttpStatusCode.BadRequest,
            Code.TooManyRequests => HttpStatusCode.TooManyRequests,
            _ => HttpStatusCode.InternalServerError,
        };
    }

    public static LogLevel ConvertToLogLevel(this AiurResponse response)
    {
        return response.Code switch
        {
            Code.Success => LogLevel.Information,
            Code.WrongKey => LogLevel.Warning,
            Code.PlaceHolder2 => LogLevel.Warning,
            Code.PlaceHolder3 => LogLevel.Warning,
            Code.NotFound => LogLevel.Warning,
            Code.UnknownError => LogLevel.Error,
            Code.RemoteNotAccessible => LogLevel.Error,
            Code.Conflict => LogLevel.Warning,
            Code.Unauthorized => LogLevel.Warning,
            Code.Timeout => LogLevel.Warning,
            Code.InvalidInput => LogLevel.Warning,
            Code.TooManyRequests => LogLevel.Warning,
            _ => LogLevel.Error,
        };
    }
}
