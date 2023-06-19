using System.Net;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Aiursoft.AiurProtocol.Attributes;
using Aiursoft.AiurProtocol.Exceptions;
using Aiursoft.AiurProtocol.Models;
using Aiursoft.Canon;
using Aiursoft.Scanner.Abstract;
using Microsoft.Extensions.Logging;

namespace Aiursoft.AiurProtocol.Services;

public class AiurApiClient : IScopedDependency
{
    private readonly HttpClient _client;
    private readonly RetryEngine _retryEngine;
    private readonly ILogger<AiurApiClient> _logger;
    private readonly Regex _regex;

    public AiurApiClient(
        RetryEngine retryEngine,
        IHttpClientFactory clientFactory,
        ILogger<AiurApiClient> logger)
    {
        _regex = new Regex("^https://", RegexOptions.Compiled);
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
        bool forceHttp = false, 
        bool autoRetry = true)
        where T : AiurResponse
    {
        if (forceHttp && !apiEndpoint.IsLocalhost())
        {
            apiEndpoint.Address = _regex.Replace(apiEndpoint.Address, "http://");
        }

        var request = new HttpRequestMessage(HttpMethod.Get, apiEndpoint.ToString())
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>())
        };

        request.Headers.Add("X-Forwarded-Proto", "https");
        request.Headers.Add("accept", "application/json, text/html");

        using var response = autoRetry ? await SendWithRetry(request) : await _client.SendAsync(request);
        return await ProcessResponse<T>(response);
    }

    public async Task<T> Post<T>(
        AiurApiEndpoint apiEndpoint, 
        ApiPayload payload, 
        SendMode mode = SendMode.HttpForm, 
        bool forceHttp = false,
        bool autoRetry = true) where T : AiurResponse
    {
        if (forceHttp && !apiEndpoint.IsLocalhost())
        {
            apiEndpoint.Address = _regex.Replace(apiEndpoint.Address, "http://");
        }

        var request = new HttpRequestMessage(HttpMethod.Post, apiEndpoint.ToString())
        {
            Content = mode == SendMode.HttpForm ? 
                new FormUrlEncodedContent(payload.Params) :
                JsonContent.Create(payload.Param)
        };

        request.Headers.Add("X-Forwarded-Proto", "https");
        request.Headers.Add("accept", "application/json");

        using var response = autoRetry ? await SendWithRetry(request) : await _client.SendAsync(request);
        return await ProcessResponse<T>(response);
    }

    private async Task<T> ProcessResponse<T>(HttpResponseMessage response) where T : AiurResponse
    {
        var content = await response.Content.ReadAsStringAsync();
        if (content.IsValidJson(out T? jsonObject))
        {
            if (jsonObject == null || jsonObject.Code != ErrorType.Success || !response.IsSuccessStatusCode)
            {
                throw new AiurServerException(jsonObject ??
                                              throw new InvalidOperationException(
                                                  "Failed to deserialize the AiurResponse."));
            }

            return jsonObject;
        }

        if (response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"The {nameof(AiurApiClient)} can only handle JSON content while the remote server returned unexpected content: {content.SafeTakeFirst(100)}.");
        }

        throw new WebException(
            $"The remote server returned unexpected content: {content.SafeTakeFirst(100)}. code: {response.StatusCode} - {response.ReasonPhrase}.");
    }
}