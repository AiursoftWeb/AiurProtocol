# How to use AiurProtocol to post HTTP with JSON body

Build your SDK:

```csharp
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DemoApiApp.Sdk.Models.ApiAddressModels;

public class RegisterAddressModel
{
    [Required]
    [MaxLength(10)]
    [JsonProperty(PropertyName = "uname")]
    public string? Name { get; set; }

    [Required]
    public string? Password { get; set; }
}

public class RegisterViewModel : AiurResponse
{
    public string? UserId { get; set; }
}
```

Now build your server:

```csharp
// In Controller:
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

Now write your call SDK:

```csharp
// In SDK:
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
```

And you can call it now:

```csharp
// To use the SDK to call the server:
var result = await sdk?.RegisterJson("anduin", "Password@1234")!;
```
