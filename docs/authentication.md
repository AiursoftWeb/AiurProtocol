# Authentication and Custom Headers

If your API requires authentication (e.g., JWT Bearer token or API Key), you can pass custom headers to the `AiurProtocolClient` methods.

## Passing Headers in SDK

When implementing your SDK access class, you can accept a token and pass it in the `headers` parameter of `Get`, `Post`, etc.

```csharp
public class MySdk(AiurProtocolClient http, IOptions<MyConfig> config)
{
    public async Task<MyResponse> GetPrivateDataAsync(string accessToken)
    {
        var url = new AiurApiEndpoint(config.Value.Instance, "Data", "Private", new { });
        
        // Pass the Authorization header here
        var headers = new Dictionary<string, string>
        {
            { "Authorization", $"Bearer {accessToken}" }
        };
        
        return await http.Get<MyResponse>(url, headers: headers);
    }
}
```

## Best Practice for SDK Developers

Usually, it's better to store the token in your SDK class or provide it via a provider:

```csharp
public class MySdk(AiurProtocolClient http)
{
    public string? Token { get; set; }

    public async Task<MyResponse> GetDataAsync()
    {
        var url = new AiurApiEndpoint(...);
        var headers = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(Token))
        {
            headers.Add("Authorization", $"Bearer {Token}");
        }
        return await http.Get<MyResponse>(url, headers: headers);
    }
}
```
