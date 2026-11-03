using System.Net;
using System.Net.Http.Headers;
using Aiursoft.AiurProtocol;
using Aiursoft.AiurProtocol.Exceptions;
using Aiursoft.AiurProtocol.Models;
using Aiursoft.AiurProtocol.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DemoApiApp.Tests;

[TestClass]
public sealed class HttpErrorTests
{
    [TestMethod]
    [DataRow(401)]
    [DataRow(403)]
    [DataRow(409)]
    [DataRow(429)]
    [DataRow(500)]
    [DataRow(502)]
    [DataRow(503)]
    [DataRow(504)]
    public async Task NonProtocolErrorsRetainStatusAndWebExceptionCompatibility(int status)
    {
        using var handler = new ErrorHandler((HttpStatusCode)status);
        await using var services = BuildProvider(handler);
        var exception = await Assert.ThrowsAsync<AiurHttpResponseException>(() =>
            services.GetRequiredService<AiurProtocolClient>().Get<AiurResponse>(
                new AiurApiEndpoint("https://alerts.example", "/incidents", new { }), autoRetry: false));
        Assert.IsInstanceOfType<WebException>(exception);
        Assert.AreEqual((HttpStatusCode)status, exception.StatusCode);
        Assert.AreEqual(TimeSpan.FromSeconds(12), exception.RetryAfter);
        Assert.AreEqual(1, handler.Calls);
    }

    [TestMethod]
    public async Task ProtocolInputErrorsKeepTheirExistingExceptionType()
    {
        using var handler = new ErrorHandler(HttpStatusCode.BadRequest,
            """{"Code":-10,"Message":"Invalid input","Items":["Invalid title"],"ProtocolVersion":"10.0.3"}""");
        await using var services = BuildProvider(handler);
        await Assert.ThrowsAsync<AiurBadApiInputException>(() =>
            services.GetRequiredService<AiurProtocolClient>().Get<AiurResponse>(
                new AiurApiEndpoint("https://alerts.example", "/incidents", new { }), autoRetry: false));
        Assert.AreEqual(1, handler.Calls);
    }

    [TestMethod]
    [DataRow(401, -1)]
    [DataRow(409, -7)]
    [DataRow(429, -11)]
    [DataRow(503, -5)]
    public async Task ProtocolErrorsKeepTheirTypeAndExposeHttpContextAsInnerException(int status, int code)
    {
        using var handler = new ErrorHandler((HttpStatusCode)status,
            $$"""{"Code":{{code}},"Message":"API failed","ProtocolVersion":"10.0.3"}""");
        await using var services = BuildProvider(handler);
        var exception = await Assert.ThrowsAsync<AiurUnexpectedServerResponseException>(() =>
            services.GetRequiredService<AiurProtocolClient>().Get<AiurResponse>(
                new AiurApiEndpoint("https://alerts.example", "/incidents", new { }), autoRetry: false));
        Assert.AreEqual((Code)code, exception.Response.Code);
        var context = exception.InnerException as AiurHttpResponseException;
        Assert.IsNotNull(context);
        Assert.AreEqual((HttpStatusCode)status, context.StatusCode);
        Assert.AreEqual(TimeSpan.FromSeconds(12), context.RetryAfter);
        Assert.AreEqual(1, handler.Calls);
    }

    private static ServiceProvider BuildProvider(HttpMessageHandler handler)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAiurProtocolClient();
        services.AddSingleton<IHttpClientFactory>(new ClientFactory(handler));
        return services.BuildServiceProvider();
    }

    private sealed class ClientFactory(HttpMessageHandler handler) : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => new(handler, disposeHandler: false);
    }

    private sealed class ErrorHandler(HttpStatusCode status, string body = "upstream unavailable") : HttpMessageHandler
    {
        public int Calls { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Calls++;
            var response = new HttpResponseMessage(status) { Content = new StringContent(body) };
            response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(12));
            return Task.FromResult(response);
        }
    }
}
