# HTTP Status code translation

The AiurProtocol.Code will be translated to HTTP status code.

You can check the source code here: [src\Aiursoft.AiurProtocol\Services\StatusCodeTranslator.cs](../src/Aiursoft.AiurProtocol/Services/StatusCodeTranslator.cs).

```csharp
return response.Code switch
{
    Code.Success => HttpStatusCode.OK,
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

| Code        | Description    |  solution  |
|--|--|--|
|0 | Request completed successfully | No correction required
|-1 | Wrong key. | Check whether a legal key is passed
|-2 | Request pending | An operation with the same meaning is already in progress. Please try again later.
|-3 | Cautions | The operation has been completed, but still needs attention. Read the value of the message parameter
|-4 | Not found | The target object of the operation does not exist. Please confirm that the target exists
|-5 | Server crash | Server unknown error. Please submit feedback to the server team
|-6 | Has been executed | An operation with the same meaning has been executed. No further resolution is needed.
|-7 | There are not enough resources | The available resources cannot meet the operation requirements. Please check the rationality of the request.
|-8 | Unauthorized | The user cannot pass the authentication or does not have the permission to perform the operation. Make sure that the user's permissions are normal.
|-10 | The input value type is invalid | The parameter is missing, or the parameter passed in does not conform to the specification. Check the parameters.
|-11 | Timeout | The request has been waiting for a long time in processing and cannot be responded. Please submit feedback to the server team.