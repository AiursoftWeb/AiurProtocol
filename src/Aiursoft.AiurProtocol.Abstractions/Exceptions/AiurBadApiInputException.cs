using System.Net;
using Aiursoft.AiurProtocol.Models;

namespace Aiursoft.AiurProtocol.Exceptions;

public class AiurBadApiInputException : AiurUnexpectedServerResponseException
{
    public IReadOnlyCollection<string> Reasons { get; set; }

    public AiurBadApiInputException(AiurCollection<string> response)
        : base(response, new AggregateException("Multiple API input error.",
            response.Items?.Select(i => new InvalidOperationException(i)) 
            ?? throw new WebException("Failed to parse API response.")))
    {
        if (response.Code != Code.InvalidInput)
        {
            throw new InvalidOperationException(
                $"The exception with type: '{nameof(AiurBadApiInputException)}' should not be thrown because the server returned result: {response.Code}.");
        }
        
        Reasons = response.Items ?? throw new WebException("Failed to parse remote API response.");
    }
}