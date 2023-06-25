# Collection result paging

It's simple to return a paged Collection.

First, build an address model for your SDK:

```csharp
using Aiursoft.AiurProtocol.Interfaces;

namespace DemoApiApp.Sdk.Models.ApiAddressModels;

public class QueryNumberAddressModel : Pager
{
    public string? Question { get; set; }
}
```

We need to build a fake database for our API. This is only for demo purpose.

```csharp
// On server:
private IEnumerable<int> Fibonacci()
{
    int current = 1, next = 1;

    while (true)
    {
        yield return current;
        next = current + (current = next);
    }
}
```

Now you can build your API. Write the following method in your controller:

```csharp
// In controller:
public async Task<IActionResult> QuerySomethingPaged([FromQuery]QueryNumberAddressModel model)
{
    // Here, using Fibonacci as a sample database.
    var database = Fibonacci()
        .Take(30)
        .AsQueryable();

    var items = database
        .Where(i => i.ToString().EndsWith(model.Question ?? string.Empty))
        .AsQueryable()
        .OrderBy(i => i);

    // After you built the query, you can simply call this method to return a paged result.
    return await this.Protocol(Code.ResultShown, "Got your value!", items, model);
}
```

Now when you are building the SDK, you can call your API with a paged result:

```csharp
// In SDK:
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
```

For your client side:

```csharp
// To use the SDK:
var result = await sdk.QuerySomethingPagedAsync("1", pageSize: 5, pageNumber: 3);
Console.Write(result.TotalCount); // 30
```
