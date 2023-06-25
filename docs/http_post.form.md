# How to use AiurProtocol to post HTTP with HTML form body

Build your SDK:

```csharp
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DemoApiApp.Sdk.Models.ApiAddressModels;

public class RegisterAddressModel
{
    [Required]
    [MaxLength(10)]
    [FromForm(Name = "user-name")]
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
public IActionResult RegisterForm([FromForm] RegisterAddressModel model)
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
        var result = await _http.Post<RegisterViewModel>(url, form, BodyFormat.HttpFormBody);
        return result;
    }
```

And you can call it now:

```csharp
var result = await sdk?.RegisterForm("anduin", "Password@1234")!;
```
