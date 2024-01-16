using System.Diagnostics.CodeAnalysis;
using Aiursoft.AiurProtocol;
using Aiursoft.AiurProtocol.Models;
using Aiursoft.AiurProtocol.Services;
using DemoApiApp.Sdk.Models.ApiAddressModels;
using DemoApiApp.Sdk.Models.ApiViewModels;
using Microsoft.Extensions.Options;

namespace DemoApiApp.Sdk;

public class DemoAccess
{
    private readonly AiurProtocolClient _http;
    private readonly DemoServerConfig _demoServerLocator;

    public DemoAccess(
        AiurProtocolClient http,
        IOptions<DemoServerConfig> demoServerLocator)
    {
        _http = http;
        _demoServerLocator = demoServerLocator.Value;
    }

    public async Task<AiurResponse> IndexAsync()
    {
        var url = new AiurApiEndpoint(_demoServerLocator.Instance, route: "/api/hello-world", param: new {});
        var result = await _http.Get<AiurResponse>(url);
        return result;
    }
    
    public async Task<AiurValue<int>> WithRoute(int id)
    {
        var url = new AiurApiEndpoint(_demoServerLocator.Instance, route: "/api/ids/{Id}", param: new SampleRouteAddressModel
        {
            Id = id
        });
        var result = await _http.Get<AiurValue<int>>(url);
        return result;
    }

    public async Task<AiurPagedCollection<int>> RedirectAsync()
    {
        var url = new AiurApiEndpoint(_demoServerLocator.Instance, "Home", "Redirect", new { });
        var result = await _http.Get<AiurPagedCollection<int>>(url);
        return result;
    }

    [ExcludeFromCodeCoverage]
    public async Task<AiurResponse> InvalidResponseShouldNotSuccessAsync()
    {
        var url = new AiurApiEndpoint(_demoServerLocator.Instance, "Home", "InvalidResponseShouldNotSuccess", new { });
        var result = await _http.Get<AiurResponse>(url);
        return result;
    }

    public async Task<AiurValue<int>> GetANumberAsync()
    {
        var url = new AiurApiEndpoint(_demoServerLocator.Instance, "Home", "GetANumber", new { });
        var result = await _http.Get<AiurValue<int>>(url);
        return result;
    }

    public async Task<AiurCollection<int>> QuerySomethingAsync(string question)
    {
        var url = new AiurApiEndpoint(_demoServerLocator.Instance, "Home", "QuerySomething", new
        {
            question
        });
        var result = await _http.Get<AiurCollection<int>>(url);
        return result;
    }
    
    public async Task<AiurPagedCollection<int>> QuerySomethingPagedAsync(string question, int pageSize, int pageNumber)
    {
        var url = new AiurApiEndpoint(_demoServerLocator.Instance, "Home", "QuerySomethingPaged", new QueryNumberAddressModel
        {
            Question = question,
            PageSize = pageSize,
            PageNumber = pageNumber
        });
        var result = await _http.Get<AiurPagedCollection<int>>(url);
        return result;
    }

    public async Task<AiurCollection<int>> GetFibonacciFirst10Async()
    {
        var url = new AiurApiEndpoint(_demoServerLocator.Instance, "Home", "GetFibonacciFirst10", new { });
        var result = await _http.Get<AiurCollection<int>>(url);
        return result;
    }

    public async Task<RegisterViewModel> RegisterForm(
        string username, 
        string password)
    {
        var url = new AiurApiEndpoint(_demoServerLocator.Instance, "Home", "RegisterForm", new { });
        var form = new AiurApiPayload(new RegisterAddressModel
        {
            Name = username,
            Password = password
        });
        var result = await _http.Post<RegisterViewModel>(url, form, disableClientSideValidation: true);
        return result;
    }

    public async Task<RegisterViewModel> RegisterJson(string username, string password)
    {
        var url = new AiurApiEndpoint(_demoServerLocator.Instance, "Home", "RegisterJson", new { });
        var form = new AiurApiPayload(new RegisterAddressModel()
        {
            Name = username,
            Password = password
        });
        var result = await _http.Post<RegisterViewModel>(url, form, BodyFormat.HttpJsonBody);
        return result;
    }

    [ExcludeFromCodeCoverage]
    public async Task<AiurResponse> CrashKnownAsync()
    {
        var url = new AiurApiEndpoint(_demoServerLocator.Instance, "Home", "CrashKnown", new { });
        var result = await _http.Get<AiurResponse>(url);
        return result;
    }

    [ExcludeFromCodeCoverage]
    public async Task<AiurResponse> CrashUnknownAsync()
    {
        var url = new AiurApiEndpoint(_demoServerLocator.Instance, "Home", "CrashUnknown", new { });
        var result = await _http.Get<AiurResponse>(url);
        return result;
    }
    
    public async Task<AiurResponse> ComplicatedRoute(string accessToken, string siteName, string folderNames)
    {
        var url = new AiurApiEndpoint(_demoServerLocator.Instance, "ViewContent/{SiteName}/{**FolderNames}", new ComplicatedRouteAddressModel
        {
            AccessToken = accessToken,
            SiteName = siteName,
            FolderNames = folderNames
        });
        var result = await _http.Get<AiurResponse>(url);
        return result;
    }
}