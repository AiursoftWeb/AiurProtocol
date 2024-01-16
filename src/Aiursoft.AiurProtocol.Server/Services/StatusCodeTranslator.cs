using System.Net;
using Aiursoft.AiurProtocol.Models;
using Microsoft.Extensions.Logging;

namespace Aiursoft.AiurProtocol.Server.Services;

internal static class StatusCodeTranslator
{
    public static HttpStatusCode ConvertToHttpStatusCode(this AiurResponse response)
    {
        return response.Code switch
        {
            // Success
            Code.JobDone => HttpStatusCode.Created,
            Code.NoActionTaken => HttpStatusCode.Accepted,
            Code.ResultShown => HttpStatusCode.OK,

            // Failed
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
            // Success
            Code.JobDone => LogLevel.Information,
            Code.NoActionTaken => LogLevel.Information,
            Code.ResultShown => LogLevel.Trace,

            // Failed
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
