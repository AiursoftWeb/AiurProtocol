# Build an API with custom routing

It's simple. For your controller:

```csharp
// In Controller:
[Route("home/no-action")]
public IActionResult NoAction()
{
    return this.Protocol(Code.NoActionTaken, "No action taken!");
}
```

Now when you are building the SDK, use the correct route:

```csharp
// In SDK:
public async Task<AiurResponse> NoActionAsync()
{
    var url = new AiurApiEndpoint(_demoServerLocator.Instance, "Home", "no-action", new { });
    var result = await _http.Get<AiurResponse>(url);
    return result;
}
```

Now call it in your client!

```csharp
// To use the SDK to call the server:
var result = await sdk.NoActionAsync();
```
