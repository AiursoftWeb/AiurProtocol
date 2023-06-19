﻿using System.Net;
using System.Text.RegularExpressions;
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

    public async Task<string> Get(AiurUrl url, bool forceHttp = false, bool autoRetry = true)
    {
        if (forceHttp && !url.IsLocalhost())
        {
            url.Address = _regex.Replace(url.Address, "http://");
        }

        var request = new HttpRequestMessage(HttpMethod.Get, url.ToString())
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>())
        };

        request.Headers.Add("X-Forwarded-Proto", "https");
        request.Headers.Add("accept", "application/json, text/html");

        using var response = autoRetry ? await SendWithRetry(request) : await _client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        if (content.IsValidJson())
        {
            return content;
        }

        if (response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"The {nameof(AiurApiClient)} can only handle JSON content while the remote server returned unexpected content: {content.SafeTakeFirst(100)}.");
        }

        throw new WebException(
            $"The remote server returned unexpected content: {content.SafeTakeFirst(100)}. code: {response.StatusCode} - {response.ReasonPhrase}.");
    }

    public async Task<string> Post(AiurUrl url, AiurUrl postDataStr, bool forceHttp = false, bool autoRetry = true)
    {
        if (forceHttp && !url.IsLocalhost())
        {
            url.Address = _regex.Replace(url.Address, "http://");
        }

        var request = new HttpRequestMessage(HttpMethod.Post, url.ToString())
        {
            Content = new FormUrlEncodedContent(postDataStr.Params)
        };

        request.Headers.Add("X-Forwarded-Proto", "https");
        request.Headers.Add("accept", "application/json");

        using var response = autoRetry ? await SendWithRetry(request) : await _client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        if (content.IsValidJson())
        {
            return content;
        }

        if (response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"The {nameof(AiurApiClient)} can only handle JSON content while the remote server returned unexpected content: {content.SafeTakeFirst(100)}.");
        }

        throw new WebException(
            $"The remote server returned unexpected content: {content.SafeTakeFirst(100)}. code: {response.StatusCode} - {response.ReasonPhrase}.");
    }

    public async Task<string> PostWithFile(AiurUrl url, Stream fileStream, bool forceHttp = false,
        bool autoRetry = true)
    {
        if (forceHttp && !url.IsLocalhost())
        {
            url.Address = _regex.Replace(url.Address, "http://");
        }

        var request = new HttpRequestMessage(HttpMethod.Post, url.Address)
        {
            Content = new MultipartFormDataContent
            {
                { new StreamContent(fileStream), "file", "file" }
            }
        };

        request.Headers.Add("X-Forwarded-Proto", "https");
        request.Headers.Add("accept", "application/json");

        using var response = autoRetry ? await SendWithRetry(request) : await _client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        if (content.IsValidJson())
        {
            return content;
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