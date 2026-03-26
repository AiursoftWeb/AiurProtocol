# Build an API with custom routing

AiurProtocol supports ASP.NET Core custom routing. You can use `[Route]` attribute to define your API address.

## Simple custom routing

For your controller:

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

## Route with parameters

You can also use route parameters like `{Id}` in your route template.

For your controller:

```csharp
// In Controller:
[Route("/api/ids/{Id}")]
public IActionResult WithRoute(SampleRouteAddressModel model)
{
    return this.Protocol(Code.ResultShown, "Got your number!", value: model.Id);
}
```

In your SDK, you need to provide the `Id` in your address model:

```csharp
// In SDK Address Model:
public class SampleRouteAddressModel
{
    public int Id { get; set; }
}

// In SDK Access Class:
public async Task<AiurValue<int>> WithRoute(int id)
{
    var url = new AiurApiEndpoint(_demoServerLocator.Instance, route: "/api/ids/{Id}", param: new SampleRouteAddressModel
    {
        Id = id
    });
    var result = await _http.Get<AiurValue<int>>(url);
    return result;
}
```

## Advanced routing with catch-all

AiurProtocol also supports catch-all parameters like `{**FolderNames}`.

For your controller:

```csharp
// In Controller:
[Route("ViewContent/{SiteName}/{**FolderNames}")]
public IActionResult ComplicatedRoute(ComplicatedRouteAddressModel model)
{
    if (model.SiteName == "site@name" && 
        model.FolderNames == "folder1/folder2/folder3" &&
        model.AccessToken == "token")
    {
        return this.Protocol(Code.NoActionTaken, "Success!");
    }
    else
    {
        throw new AiurServerException(Code.WrongKey, "Unexpected request value!");
    }
}
```

In your SDK:

```csharp
// In SDK Address Model:
public class ComplicatedRouteAddressModel
{
    public string? AccessToken { get; set; }
    public string? SiteName { get; set; }
    public string? FolderNames { get; set; }
}

// In SDK Access Class:
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
```

## Usage

Now you can call them in your client!

```csharp
// To use the SDK to call the server:
var result1 = await sdk.NoActionAsync();
var result2 = await sdk.WithRoute(123);
var result3 = await sdk.ComplicatedRoute("token", "site@name", "folder1/folder2/folder3");
```
