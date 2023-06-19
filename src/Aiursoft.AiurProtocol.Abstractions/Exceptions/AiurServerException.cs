using Aiursoft.AiurProtocol.Models;

namespace Aiursoft.AiurProtocol.Exceptions;

/// <summary>
///     Throw this exception in any methods called from API. This will stop the controller logic.
///     Use together with `AiurExpAiurProtocol` will directly return the message as `AiurProtocol`.
/// </summary>
public class AiurServerException : Exception
{
    public AiurServerException(ErrorType code, string message) : base(message)
    {
        Code = code;
    }

    public ErrorType Code { get; }
}