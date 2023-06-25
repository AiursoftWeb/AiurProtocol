# HTTP Status code translation

The AiurProtocol.Code will be translated to HTTP status code.

You can check the source code here: [src\Aiursoft.AiurProtocol\Services\StatusCodeTranslator.cs](../src/Aiursoft.AiurProtocol/Services/StatusCodeTranslator.cs).

```csharp
return response.Code switch
{
    // Success
    Code.JobDone => HttpStatusCode.Created,
    Code.NoActionTaken => HttpStatusCode.Accepted,
    Code.ResultShown => HttpStatusCode.OK,

    // Failed
    Code.WrongKey => HttpStatusCode.Unauthorized,
    Code.PlaceHolder2 => HttpStatusCode.Gone,
    Code.PlaceHolder3 => HttpStatusCode.Gone,
    Code.NotFound => HttpStatusCode.NotFound,
    Code.UnknownError => HttpStatusCode.InternalServerError,
    Code.RemoteNotAccessible => HttpStatusCode.InternalServerError,
    Code.Conflict => HttpStatusCode.Conflict,
    Code.Unauthorized => HttpStatusCode.Unauthorized,
    Code.Timeout => HttpStatusCode.RequestTimeout,
    Code.InvalidInput => HttpStatusCode.BadRequest,
    Code.TooManyRequests => HttpStatusCode.TooManyRequests,
    _ => HttpStatusCode.InternalServerError,
};
```

| Code | Http Status Code | Explanation | Solution |
|------|------------------|-------------|----------|
|JobDone|Created|The job was finished.|N/A|
|NoActionTaken|Accepted|The job seems require no action to be taken|Check the job context.|
|ResultShown|OK|The result you are looking for was found and shown.|N/A|
|WrongKey|Unauthorized|The key was wrong.|Check your key.|
|PlaceHolder2|Gone|A place holder for further usage.|N/A|
|PlaceHolder3|Gone|A place holder for further usage.|N/A|
|NotFound|NotFound|The item you are looking for was not found.|Check your input object id.|
|UnknownError|InternalServerError|The server encountered an unknown error.|Check the server log.|
|RemoteNotAccessible|InternalServerError|A remote resource was not accessible.|Check the server log.|
|Conflict|Conflict|The item you are trying to create was already exists.|Check your input object id.|
|Unauthorized|Unauthorized|You are not authorized to do this action.|Check your permission.|
|Timeout|RequestTimeout|The server was timeout.|Contact the server owner.|
|InvalidInput|BadRequest|The input was invalid.|Check your input.|
|TooManyRequests|TooManyRequests|You have sent too many requests to the server.|Wait for a while.|
