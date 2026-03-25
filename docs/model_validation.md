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
        return this.Protocol(new { UserId = "user-1" });
    }
}
```

### Client Response
If a client sends a request missing the `Name` field, they will receive a response like:

```json
{
    "code": 400,
    "message": "Multiple errors were found in the API input. (1 errors)",
    "items": [
        "The Name field is required."
    ]
}
```

On the client side, if using `AiurProtocolClient`, this will throw an `AiurBadApiInputException` with the list of reasons.
