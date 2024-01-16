using System.Net;
using Aiursoft.AiurProtocol;
using Aiursoft.AiurProtocol.Exceptions;
using Aiursoft.AiurProtocol.Models;
using Aiursoft.CSTools.Tools;
using Aiursoft.WebTools;
using DemoApiApp.Sdk;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoApiApp.Tests;

[TestClass]
public class IntegrationTests
{
    private readonly int _port;
    private readonly string _endpointUrl;
    private IHost? _server;
    private ServiceProvider? _serviceProvider;

    public IntegrationTests()
    {
        _port = Network.GetAvailablePort();
        _endpointUrl = $"http://localhost:{_port}";
    }

    [TestInitialize]
    public async Task TestInitialize()
    {
        _server = Extends.App<Startup>(Array.Empty<string>(), port: _port);
        await _server.StartAsync();

        var services = new ServiceCollection();
        services.AddDemoService(_endpointUrl);
        _serviceProvider = services.BuildServiceProvider();
    }

    [TestCleanup]
    public async Task CleanServer()
    {
        if (_server != null)
        {
            await _server.StopAsync();
            _server.Dispose();
        }
    }

    [TestMethod]
    public async Task TestIndex()
    {
        var sdk = _serviceProvider?.GetRequiredService<DemoAccess>();
        var result = await sdk?.IndexAsync()!;
        Assert.AreEqual("Welcome to this API project!", result.Message);
    }
    
    [TestMethod]
    public async Task TestWithId()
    {
        var sdk = _serviceProvider?.GetRequiredService<DemoAccess>();
        var result = await sdk?.WithRoute(123)!;
        Assert.AreEqual(123, result.Value);
    }

    [TestMethod]
    public async Task TestStatusCode()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, _endpointUrl + "/home/no-action")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>())
        };

        request.Headers.Add("accept", "application/json, text/html");
        var client = new HttpClient();
        var result = await client.SendAsync(request);
        Assert.AreEqual(202, (int)result.StatusCode);
    }


    [TestMethod]
    public async Task TestRedirect()
    {
        var sdk = _serviceProvider?.GetRequiredService<DemoAccess>();
        var result = (await sdk?.RedirectAsync()!).Items!.ToArray();
        Assert.AreEqual(1, result[0]);
        Assert.AreEqual(1, result[1]);
        Assert.AreEqual(21, result[2]);
    }

    [TestMethod]
    public async Task TestInvalid()
    {
        try
        {
            var sdk = _serviceProvider?.GetRequiredService<DemoAccess>();
            _ = await sdk?.InvalidResponseShouldNotSuccessAsync()!;
            Assert.Fail("Bad test should not pass");
        }
        catch (WebException e)
        {
            Assert.AreEqual(
                @"The remote server returned unexpected error content: {""message"":""This is not a valid Protocol response.""}. code: BadRequest - Bad Request.",
                e.Message);
        }
    }

    [TestMethod]
    public async Task TestQuery()
    {
        var sdk = _serviceProvider?.GetRequiredService<DemoAccess>();
        var result = (await sdk?.QuerySomethingAsync("3")!).Items?.ToArray();
        Assert.AreEqual(3, result?[0]);
        Assert.AreEqual(13, result?[1]);
        Assert.AreEqual(233, result?[2]);
    }

    [TestMethod]
    public async Task TestQueryPaged()
    {
        var sdk = _serviceProvider?.GetRequiredService<DemoAccess>();
        var result = await sdk?.QuerySomethingPagedAsync(string.Empty, 5, 3)!;
        var resultArray = result.Items?.ToArray();
        Assert.AreEqual(5, resultArray?.Length);
        Assert.AreEqual(89, resultArray?[0]);
        Assert.AreEqual(144, resultArray?[1]);
        Assert.AreEqual(233, resultArray?[2]);
    }

    [TestMethod]
    public async Task TestQueryPagedInvalidPageSize()
    {
        try
        {
            var sdk = _serviceProvider?.GetRequiredService<DemoAccess>();
            _ = await sdk?.QuerySomethingPagedAsync(string.Empty, 101, 3)!;
        }
        catch (AiurBadApiInputException e)
        {
            Assert.AreEqual(
                "The field PageSize must be between 1 and 100.", 
                (e.InnerException as AggregateException)!.InnerExceptions.Single().Message);
            return;
        }
        Assert.Fail("Invalid request should not success!");
    }

    [TestMethod]
    public async Task TestGetANumber()
    {
        var sdk = _serviceProvider?.GetRequiredService<DemoAccess>();
        var result = await sdk?.GetANumberAsync()!;
        Assert.AreEqual(123, result.Value);
    }

    [TestMethod]
    public async Task TestGetACollection()
    {
        var sdk = _serviceProvider?.GetRequiredService<DemoAccess>();
        var result = await sdk?.GetFibonacciFirst10Async()!;
        Assert.AreEqual(10, result.Items?.Count);
    }

    [TestMethod]
    public async Task TestRegisterForm()
    {
        var sdk = _serviceProvider?.GetRequiredService<DemoAccess>();
        var result = await sdk?.RegisterForm("anduin", "Password@1234")!;
        Assert.AreEqual("your-id-anduin", result.UserId);
    }

    [TestMethod]
    public async Task TestRegisterInvalidForm()
    {
        try
        {
            var sdk = _serviceProvider?.GetRequiredService<DemoAccess>();
            _ = await sdk?.RegisterForm("12345678901", "Password@1234")!;
            Assert.Fail("Bad test should not pass");
        }
        catch (AiurBadApiInputException e)
        {
            Assert.AreEqual("Multiple errors were found in the API input. (1 errors)", e.Message);
            Assert.AreEqual(1, e.Reasons.Count);
            Assert.AreEqual("The field Name must be a string or array type with a maximum length of '10'.",
                e.Reasons.Single());
        }
    }

    [TestMethod]
    public async Task TestRegisterJson()
    {
        var sdk = _serviceProvider?.GetRequiredService<DemoAccess>();
        var result = await sdk?.RegisterJson("anduin", "Password@1234")!;
        Assert.AreEqual("your-id-anduin", result.UserId);
    }

    [TestMethod]
    public async Task TestCrashKnown()
    {
        try
        {
            var sdk = _serviceProvider?.GetRequiredService<DemoAccess>();
            _ = await sdk?.CrashKnownAsync()!;
            Assert.Fail("Bad test should not pass");
        }
        catch (AiurUnexpectedServerResponseException e)
        {
            Assert.AreEqual("Known error", e.Message);
            Assert.AreEqual(Code.Conflict, e.Response.Code);
        }
    }

    [TestMethod]
    public async Task TestCrashUnknown()
    {
        try
        {
            var sdk = _serviceProvider?.GetRequiredService<DemoAccess>();
            _ = await sdk?.CrashUnknownAsync()!;
            Assert.Fail("Bad test should not pass");
        }
        catch (AiurUnexpectedServerResponseException e)
        {
            Assert.IsTrue(e.Message.Contains("Sorry about that."));
            Assert.AreEqual(Code.UnknownError, e.Response.Code);
        }
    }
    
    [TestMethod]
    public async Task TestComplicatedRoute()
    {
        var sdk = _serviceProvider?.GetRequiredService<DemoAccess>();
        _ = await sdk?.ComplicatedRoute("token", "site@name", "folder1/folder2/folder3")!;
    }
}