# Model Validation and Request Interception

When building APIs with `Aiursoft.AiurProtocol`, you can use the built-in validation mechanism to handle invalid requests automatically.

## How it works

1.  **Server-side Attributes**: Decorate your API Controllers with `[ApiModelStateChecker]`. This attribute is an action filter that runs before your action logic.
2.  **Model Attributes**: Use standard Data Annotations like `[Required]`, `[MaxLength]`, `[EmailAddress]`, etc., on your `AddressModel` or `ViewModel`.

### Automatic Interception

When the `[ApiModelStateChecker]` attribute is present:
-   If the `ModelState` is invalid (e.g., a `[Required]` field is missing or a `[MaxLength]` is exceeded), the framework **automatically intercepts** the request.
-   The request **does not** proceed to your Controller's business logic.
-   The framework returns an `AiurCollection<string>` with `Code.InvalidInput` (which translates to a `400 Bad Request` status code).
-   The response body contains a list of error messages explaining why the validation failed.

## Example

### Model with Validation
```csharp
public class RegisterAddressModel
{
    [Required]
    [MaxLength(10)]
    public string Name { get; set; }

    [Required]
    public string Password { get; set; }
}
```

### Controller with Interception
```csharp
[ApiExceptionHandler]
[ApiModelStateChecker] // This intercepts invalid requests
public class HomeController : ControllerBase
{
    [HttpPost]
    public IActionResult Register([FromForm] RegisterAddressModel model)
    {
        // This logic will NOT execute if validation fails
        return this.Protocol(new RegisterViewModel 
        { 
            UserId = "user-1",
            Code = Code.JobDone,
            Message = "Registered."
        });
    }
}
```

### Client Response
If a client sends a request missing the `Name` field, they will receive a response like:

```json
{
    "code": -10,
    "message": "Multiple errors were found in the API input. (1 errors)",
    "items": [
        "The Name field is required."
    ]
}
```

On the client side, if using `AiurProtocolClient`, it will perform **client-side validation** by default before sending any request to the server.

### How it works

1.  **Model Validation**: Before sending a request, `AiurProtocolClient` uses `Validator.TryValidateObject` to check if your input model passes all standard Data Annotations.
2.  **Early Interception**: If validation fails on the client side, the request is **not sent** to the server.
3.  **Exception**: An `AiurBadApiInputException` is thrown immediately.

### Client-side Validation Response

When client-side validation fails, the exception message will be:

```text
Could NOT pass client side model validation! (Request not sent to server)
```

The `Reasons` property of the `AiurBadApiInputException` will contain the validation error messages.

### Disabling Client-side Validation

If you want to skip client-side validation and let the server handle it (or for testing purposes), you can set the `disableClientSideValidation` parameter to `true`:

```csharp
var result = await _http.Post<RegisterViewModel>(url, payload, disableClientSideValidation: true);
```

In this case, the server will return a response like the one shown above, and `AiurProtocolClient` will throw an `AiurBadApiInputException` with the **server's error message**: `Multiple errors were found in the API input. (1 errors)`.
