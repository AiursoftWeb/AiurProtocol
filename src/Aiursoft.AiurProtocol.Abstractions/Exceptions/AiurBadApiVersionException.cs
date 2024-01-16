using Aiursoft.AiurProtocol.Models;

namespace Aiursoft.AiurProtocol.Exceptions;

public class AiurBadApiVersionException : AiurUnexpectedServerResponseException
{
    public AiurBadApiVersionException(Version sdkVersion, AiurResponse serverResponse)
        : base(serverResponse, new InvalidOperationException($"The version of the response API is: {serverResponse.ProtocolVersion} while the AiurProtocol SDK version is {sdkVersion}."))
    {
    }
}