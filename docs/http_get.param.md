# Call HTTP Get with parameter

It's simple.

Server:

```csharp
// In Controller:
public IActionResult QuerySomething([FromQuery] string question)
{
    var items = Fibonacci()
        .Where(i => i.ToString().EndsWith(question))
        .Take(10)
        .ToList();
    return this.Protocol(Code.Success, "Got your value!", items);
}
```

SDK:

```csharp
// In SDK:
public async Task<AiurCollection<int>> QuerySomethingAsync(string question)
{
    var url = new AiurApiEndpoint(_demoServerLocator.Instance, "Home", "QuerySomething", new
    {
        question
    });
    var result = await _http.Get<AiurCollection<int>>(url);
    return result;
}
```

Now you can call it:

```csharp
// To use the SDK to call the server:
var result = await sdk.QuerySomethingAsync("3");
```
