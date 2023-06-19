using Aiursoft.AiurProtocol.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aiursoft.AiurProtocol.Services;

internal static class StatusCodeTranslator
{
    public static HttpStatusCode ConvertToHttpStatusCode(this AiurResponse response)
    {
        return response.Code switch
        {
            ErrorType.Success => HttpStatusCode.OK,
            ErrorType.WrongKey or ErrorType.Unauthorized => HttpStatusCode.Unauthorized,
            ErrorType.InsufficientPermissions => HttpStatusCode.Forbidden,
            ErrorType.Gone => HttpStatusCode.Gone,
            ErrorType.NotFound => HttpStatusCode.NotFound,
            ErrorType.UnknownError => HttpStatusCode.InternalServerError,
            ErrorType.HasSuccessAlready => HttpStatusCode.AlreadyReported,
            ErrorType.Conflict => HttpStatusCode.Conflict,
            ErrorType.InvalidInput => HttpStatusCode.BadRequest,
            ErrorType.Timeout => HttpStatusCode.RequestTimeout,
            ErrorType.TooManyRequests => HttpStatusCode.TooManyRequests,
            _ => HttpStatusCode.InternalServerError,
        };
    }
}
