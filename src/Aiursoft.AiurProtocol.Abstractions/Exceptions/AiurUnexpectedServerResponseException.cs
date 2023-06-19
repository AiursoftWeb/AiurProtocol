using System.Diagnostics.CodeAnalysis;
using System.Net;
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

public class AiurBadApiInputException : AiurUnexpectedServerResponseException
{
    public IReadOnlyCollection<string> Reasons { get; set; }

    public AiurBadApiInputException(AiurResponse response)
        : base(response, new AggregateException("Multiple API input error.",
            (response as AiurCollection<string>)
            ?.Items?.Select(i => new InvalidOperationException(i)) 
            ?? throw new WebException("Failed to parse remote API response.")))
    {
        if (response.Code != Code.InvalidInput)
        {
            throw new InvalidOperationException(
                $"The exception with type: '{nameof(AiurBadApiInputException)}' should not be thrown because the server returned result: {response.Code}.");
        }
        
        Reasons = (response as AiurCollection<string>)?.Items
                  ?? throw new WebException("Failed to parse remote API response.");
    }
}