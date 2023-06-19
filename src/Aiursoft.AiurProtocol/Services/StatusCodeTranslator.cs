using Aiursoft.AiurProtocol.Models;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Aiursoft.AiurProtocol.Services;

internal static class StatusCodeTranslator
{
    public static HttpStatusCode ConvertToHttpStatusCode(this AiurResponse response)
    {
        return response.Code switch
        {
            ErrorType.Success => HttpStatusCode.OK,
            ErrorType.WrongKey => HttpStatusCode.Unauthorized,
            ErrorType.InsufficientPermissions => HttpStatusCode.Forbidden,
            ErrorType.Gone => HttpStatusCode.Gone,
            ErrorType.NotFound => HttpStatusCode.NotFound,
            ErrorType.UnknownError => HttpStatusCode.InternalServerError,
            ErrorType.HasSuccessAlready => HttpStatusCode.AlreadyReported,
            ErrorType.Conflict => HttpStatusCode.Conflict,
            ErrorType.Unauthorized => HttpStatusCode.Unauthorized,
            ErrorType.Timeout => HttpStatusCode.RequestTimeout,
            ErrorType.InvalidInput => HttpStatusCode.BadRequest,
            ErrorType.TooManyRequests => HttpStatusCode.TooManyRequests,
            _ => HttpStatusCode.InternalServerError,
        };
    }

    public static LogLevel ConvertToLogLevel(this AiurResponse response)
    {
        return response.Code switch
        {
            ErrorType.Success => LogLevel.Information,
            ErrorType.WrongKey => LogLevel.Warning,
            ErrorType.InsufficientPermissions => LogLevel.Warning,
            ErrorType.Gone => LogLevel.Warning,
            ErrorType.NotFound => LogLevel.Warning,
            ErrorType.UnknownError => LogLevel.Error,
            ErrorType.HasSuccessAlready => LogLevel.Warning,
            ErrorType.Conflict => LogLevel.Warning,
            ErrorType.Unauthorized => LogLevel.Warning,
            ErrorType.Timeout => LogLevel.Warning,
            ErrorType.InvalidInput => LogLevel.Warning,
            ErrorType.TooManyRequests => LogLevel.Warning,
            _ => LogLevel.Error,
        };
    }
}
