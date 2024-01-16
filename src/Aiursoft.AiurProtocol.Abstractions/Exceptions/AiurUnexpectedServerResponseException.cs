using Aiursoft.AiurProtocol.Models;

namespace Aiursoft.AiurProtocol.Exceptions;

/// <summary>
///     Throw this exception if the json respond by the Aiursoft Server was not with code = 0.
///     Catch it in your own code or just use `AiurExpAiurProtocol`.
/// </summary>
public class AiurUnexpectedServerResponseException : Exception
{
    public AiurUnexpectedServerResponseException(AiurResponse response, Exception? innerException = null) 
        : base(response.Message, innerException)
    {
        Response = response;
    }

    public AiurResponse Response { get; set; }
}
