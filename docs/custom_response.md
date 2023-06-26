# Return complicated value

If you are responding an easy value, like boolen, string, int, etc. You can just return it directly:

```csharp
// In controller:
public IActionResult GetANumber()
{
    return this.Protocol(Code.ResultShown, "Got your value!", value: 123);
}
```

But if you want to return a more complicated value, like a class, you need to make your API's returning ViewModel class inherit from AiurResponse.

```csharp
// In SDK:
using Aiursoft.AiurProtocol.Models;

namespace DemoApiApp.Sdk.Models.ApiViewModels;

public class RegisterAddressModel
{
    [FromForm(Name = "user-name")]
    [JsonProperty(PropertyName = "uname")]
    [Required]
    [MaxLength(10)]
    public string? Name { get; set; }
    [Required]
    public string? Password { get; set; }
}

public class RegisterViewModel : AiurResponse
{
    public string? UserId { get; set; }
}
```

Now build your API:

```csharp
// In controller:
[HttpPost]
public IActionResult RegisterJson([FromBody] RegisterAddressModel model)
{
    return this.Protocol(new RegisterViewModel
    {
        Code = Code.JobDone,
        Message = "Registered.",
        UserId = "your-id-" + model.Name
    });
}
```

Now build your SDK:

```csharp
public async Task<RegisterViewModel> RegisterAsync(string username, string password)
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
```

Now call it in your client!

```csharp
// To use the SDK to call the server:
var result = await sdk.RegisterAsync("test", "test");
```

That's it! You can now return any complicated value from your API!
