using System.Net;

namespace Aiursoft.AiurProtocol.Exceptions;

/// <summary>
/// An unsuccessful HTTP response that does not contain an AiurProtocol result.
/// </summary>
public sealed class AiurHttpResponseException(
    HttpStatusCode statusCode, string message, TimeSpan? retryAfter = null) : WebException(message)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
    public TimeSpan? RetryAfter { get; } = retryAfter;
}
