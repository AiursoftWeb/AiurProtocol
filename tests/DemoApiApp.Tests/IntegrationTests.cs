using Aiursoft.WebTools;
using Aiursoft.XelNaga.Tools;
using DemoApiApp.Sdk;
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
        _server = Extends.App<Startup>(port: _port);
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
    public async Task TestInvalid()
    {
        try
        {
            var sdk = _serviceProvider?.GetRequiredService<DemoAccess>();
            _ = await sdk?.InvalidResponseShouldNotSuccessAsync()!;
            Assert.Fail("Bad test should not pass");
        }
        catch (Exception e)
        {
            Assert.AreEqual("Required property 'code' not found in JSON. Path '', line 1, position 52.", e.Message);
        }
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
    public async Task TestRegisterJson()
    {
        var sdk = _serviceProvider?.GetRequiredService<DemoAccess>();
        var result = await sdk?.RegisterJson("anduin", "Password@1234")!;
        Assert.AreEqual("your-id-anduin", result.UserId);
    }

}