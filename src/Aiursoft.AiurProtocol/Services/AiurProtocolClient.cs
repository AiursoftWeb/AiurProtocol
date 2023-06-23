using System.IO.Compression;
using System.Net;
using System.Text;
using Aiursoft.AiurProtocol.Abstractions.Configuration;
using Aiursoft.AiurProtocol.Attributes;
using Aiursoft.AiurProtocol.Exceptions;
using Aiursoft.AiurProtocol.Models;
using Aiursoft.Canon;
using Aiursoft.Scanner.Abstract;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Aiursoft.AiurProtocol.Services;

public class AiurProtocolClient : IScopedDependency
{
    private readonly HttpClient _client;
    private readonly RetryEngine _retryEngine;
    private readonly ILogger<AiurProtocolClient> _logger;

    public AiurProtocolClient(
        RetryEngine retryEngine,
        IHttpClientFactory clientFactory,
        ILogger<AiurProtocolClient> logger)
    {
        _client = clientFactory.CreateClient();
        _retryEngine = retryEngine;
        _logger = logger;
    }

    private Task<HttpResponseMessage> SendWithRetry(HttpRequestMessage request)
    {
        return _retryEngine.RunWithRetry(async _ =>
            {
                var response = await _client.SendAsync(request);
                if (response.StatusCode is HttpStatusCode.BadGateway or HttpStatusCode.ServiceUnavailable)
                {
                    throw new WebException(
                        $"Api proxy failed because of bad gateway [{response.StatusCode}]. (This error will trigger auto retry)");
                }

                return response;
            },
            when: e => e is WebException,
            onError: e => { _logger.LogWarning(e, "Transient issue (retry available) happened with remote server"); });
    }

    public async Task<T> Get<T>(
        AiurApiEndpoint apiEndpoint,
        bool autoRetry = true)
        where T : AiurResponse
    {
        var request = new HttpRequestMessage(HttpMethod.Get, apiEndpoint.ToString())
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>())
        };

        request.Headers.Add("accept", "application/json, text/html");

        using var response = autoRetry ? await SendWithRetry(request) : await _client.SendAsync(request);
        return await ProcessResponse<T>(response);
    }

    public async Task<T> Post<T>(
        AiurApiEndpoint apiEndpoint,
        ApiPayload payload,
        BodyFormat format = BodyFormat.HttpFormBody,
        bool autoRetry = true) where T : AiurResponse
    {
        var request = new HttpRequestMessage(HttpMethod.Post, apiEndpoint.ToString())
        {
            Content = format == BodyFormat.HttpFormBody
                ? new FormUrlEncodedContent(payload.Params)
                : new StringContent(JsonConvert.SerializeObject(payload.Param, ProtocolConsts.JsonSettings), Encoding.UTF8, "application/json")
        };

        request.Headers.Add("accept", "application/json");

        using var response = autoRetry ? await SendWithRetry(request) : await _client.SendAsync(request);
        return await ProcessResponse<T>(response);
    }

    private async Task<T> ProcessResponse<T>(HttpResponseMessage response) where T : AiurResponse
    {
        var content = await GetResponseContent(response);
        if (content.IsValidResponse(out AiurResponse? responseObject))
        {
            switch (responseObject?.Code)
            {
                case Code.Success or Code.NoActionNeeded:
                {
                    // Success.
                    var model = JsonConvert.DeserializeObject<T>(content, ProtocolConsts.JsonSettings)!;
                    return model;
                }
                case Code.InvalidInput:
                {
                    // Invalid input.
                    var model = JsonConvert.DeserializeObject<AiurCollection<string>>(content, ProtocolConsts.JsonSettings)!;
                    throw new AiurBadApiInputException(model);
                }
                default:
                {
                    // Other errors.
                    var model = JsonConvert.DeserializeObject<AiurResponse>(content, ProtocolConsts.JsonSettings)!;
                    throw new AiurUnexpectedServerResponseException(model);
                }
            }
        }

        if (response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"The {nameof(AiurProtocolClient)} can only handle AiurProtocol content while the remote server returned content: {content.SafeTakeFirst(100)}.");
        }

        throw new WebException(
            $"The remote server returned unexpected error content: {content.SafeTakeFirst(100)}. code: {response.StatusCode} - {response.ReasonPhrase}.");
    }
    
    private static async Task<string> GetResponseContent(HttpResponseMessage response)
    {
        var isGZipEncoded = response.Content.Headers.ContentEncoding.Contains("gzip");
        if (isGZipEncoded)
        {
            await using var stream = await response.Content.ReadAsStreamAsync();
            await using var decompressionStream = new GZipStream(stream, CompressionMode.Decompress);
            using var reader = new StreamReader(decompressionStream);
            var text = await reader.ReadToEndAsync();
            return text;
        }
        else
        {
            var text = await response.Content.ReadAsStringAsync();
            return text;
        }
    }
}