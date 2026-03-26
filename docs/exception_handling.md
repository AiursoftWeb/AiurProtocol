# Exception Handling

When using the SDK built with AiurProtocol, it's important to understand how the SDK handles different server responses.

## Success Responses

If the server returns a successful status code (like `Code.JobDone`, `Code.NoActionTaken`, or `Code.ResultShown`), the SDK method will return the corresponding response object.

```csharp
var result = await sdk.QuerySomethingAsync("question");
// result is a valid AiurResponse (or inherited) object
```

## Error Responses

If the server returns an error status code, the SDK will **throw an exception** rather than returning a response object with an error code. This allows you to use standard C# `try-catch` blocks to handle errors.

### 1. AiurBadApiInputException

Thrown when the server returns `Code.InvalidInput` (or if client-side validation fails). This typically means some fields in your request are invalid.

```csharp
try
{
    await sdk.RegisterAsync("too-long-username", "password");
}
catch (AiurBadApiInputException e)
{
    foreach (var reason in e.Reasons)
    {
        Console.WriteLine(reason);
    }
}
```

### 2. AiurUnexpectedServerResponseException

Thrown for all other error codes returned by the server, such as:
- `Code.NotFound` (404)
- `Code.Unauthorized` (401)
- `Code.Conflict` (409)
- `Code.UnknownError` (500)

```csharp
try
{
    await sdk.GetPrivateDataAsync();
}
catch (AiurUnexpectedServerResponseException e)
{
    if (e.Response.Code == Code.Unauthorized)
    {
        // Handle unauthorized access
    }
    else if (e.Response.Code == Code.NotFound)
    {
        // Handle not found
    }
    else
    {
        // Handle other errors
        Console.WriteLine(e.Response.Message);
    }
}
```

### 3. WebException

Thrown when a low-level network error occurs, or if the server returns a response that does not follow the AiurProtocol format.
