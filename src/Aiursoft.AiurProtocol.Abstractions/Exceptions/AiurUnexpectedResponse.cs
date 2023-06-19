using System;
using Aiursoft.AiurProtocol.Models;

namespace Aiursoft.AiurProtocol.Exceptions;

/// <summary>
///     Throw this exception if the json respond by the Aiursoft Server was not with code = 0.
///     Catch it in your own code or just use `AiurExpAiurProtocol`.
/// </summary>
public class AiurUnexpectedResponse : Exception
{
    public AiurUnexpectedResponse(Models.AiurResponse response) : base(response.Message)
    {
        Response = response;
    }

    public Models.AiurResponse Response { get; set; }
    public ErrorType Code => Response.Code;
}