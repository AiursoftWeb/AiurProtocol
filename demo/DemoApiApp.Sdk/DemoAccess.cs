using Aiursoft.AiurProtocol;
using Aiursoft.AiurProtocol.Attributes;
using Aiursoft.AiurProtocol.Models;
using Aiursoft.AiurProtocol.Services;
using DemoApiApp.Sdk.Models.ApiAddressModels;
using DemoApiApp.Sdk.Models.ApiViewModels;
using Microsoft.Extensions.Options;

namespace DemoApiApp.Sdk;

public class DemoAccess
{
    private readonly AiurApiClient _http;
    private readonly DemoServerConfig _observerLocator;

    public DemoAccess(
        AiurApiClient http,
        IOptions<DemoServerConfig> observerLocator)
    {
        _http = http;
        _observerLocator = observerLocator.Value;
    }

    public async Task<AiurResponse> IndexAsync()
    {
        var url = new AiurApiEndpoint(_observerLocator.Instance);
        var result = await _http.Get<AiurResponse>(url);
        return result;
    }

    public async Task<AiurResponse> InvalidResponseShouldNotSuccessAsync()
    {
        var url = new AiurApiEndpoint(_observerLocator.Instance, "/Home/InvalidResponseShouldNotSuccess", new { });
        var result = await _http.Get<AiurResponse>(url);
        return result;
    }

    public async Task<AiurValue<int>> GetANumberAsync()
    {
        var url = new AiurApiEndpoint(_observerLocator.Instance, "Home", "GetANumber", new { });
        var result = await _http.Get<AiurValue<int>>(url);
        return result;
    }

    public async Task<AiurCollection<int>> QuerySomethingAsync(string question)
    {
        var url = new AiurApiEndpoint(_observerLocator.Instance, "Home", "QuerySomething", new
        {
            question
        });
        var result = await _http.Get<AiurCollection<int>>(url);
        return result;
    }
    
    public async Task<AiurPagedCollection<int>> QuerySomethingPagedAsync(string question, int pageSize, int pageNumber)
    {
        var url = new AiurApiEndpoint(_observerLocator.Instance, "Home", "QuerySomethingPaged", new QueryNumberAddressModel
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
        var url = new AiurApiEndpoint(_observerLocator.Instance, "Home", "GetFibonacciFirst10", new { });
        var result = await _http.Get<AiurCollection<int>>(url);
        return result;
    }

    public async Task<RegisterViewModel> RegisterForm(string username, string password)
    {
        var url = new AiurApiEndpoint(_observerLocator.Instance, "Home", "RegisterForm", new { });
        var form = new ApiPayload(new RegisterAddressModel
        {
            Name = username,
            Password = password
        });
        var result = await _http.Post<RegisterViewModel>(url, form);
        return result;
    }

    public async Task<RegisterViewModel> RegisterJson(string username, string password)
    {
        var url = new AiurApiEndpoint(_observerLocator.Instance, "Home", "RegisterJson", new { });
        var form = new ApiPayload(new RegisterAddressModel()
        {
            Name = username,
            Password = password
        });
        var result = await _http.Post<RegisterViewModel>(url, form, BodyFormat.HttpJsonBody);
        return result;
    }

    public async Task<AiurResponse> CrashKnownAsync()
    {
        var url = new AiurApiEndpoint(_observerLocator.Instance, "Home", "CrashKnown", new { });
        var result = await _http.Get<AiurResponse>(url);
        return result;
    }

    public async Task<AiurResponse> CrashUnknownAsync()
    {
        var url = new AiurApiEndpoint(_observerLocator.Instance, "Home", "CrashUnknown", new { });
        var result = await _http.Get<AiurResponse>(url);
        return result;
    }
}