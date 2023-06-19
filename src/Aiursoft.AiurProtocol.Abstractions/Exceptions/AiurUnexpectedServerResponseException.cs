using Aiursoft.AiurProtocol.Models;

namespace Aiursoft.AiurProtocol.Exceptions;

/// <summary>
///     Throw this exception if the json respond by the Aiursoft Server was not with code = 0.
///     Catch it in your own code or just use `AiurExpAiurProtocol`.
/// </summary>
public class AiurUnexpectedServerResponseException : Exception
{
    public AiurUnexpectedServerResponseException(AiurResponse response, Exception? innerException = null) : base(
        response.Message, innerException)
    {
        Response = response;
    }

    public AiurResponse Response { get; set; }
    public ErrorType Code => Response.Code;
}

public class AiurBadApiInputException : AiurUnexpectedServerResponseException
{
    public IReadOnlyCollection<string> Reasons { get; set; }

    public AiurBadApiInputException(AiurResponse response)
        : base(response, new AggregateException("Multiple API input error.",
            (response as AiurCollection<string>)
            ?.Items?.Select(i => new InvalidOperationException(i)) ?? Array.Empty<InvalidOperationException>()))
    {
        Reasons = (response as AiurCollection<string>)?.Items
                  ?? throw new Exception("Failed to parse remote API response.");
    }
}