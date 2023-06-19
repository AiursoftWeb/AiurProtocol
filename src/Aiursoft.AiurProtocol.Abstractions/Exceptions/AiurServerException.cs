using Aiursoft.AiurProtocol.Models;

namespace Aiursoft.AiurProtocol.Exceptions;

/// <summary>
///     Throw this exception in any methods called from API. This will stop the controller logic.
///     Use together with `AiurExpAiurProtocol` will directly return the message as `AiurProtocol`.
/// </summary>
public class AiurServerException : Exception
{
    public AiurServerException(AiurResponse response) : base(response.Message)
    {
        Response = response;
    }

    public AiurResponse Response { get; }
}