namespace Aiursoft.AiurProtocol;

public enum Code
{
    // Success
    JobDone = 2,
    NoActionTaken = 1,
    ResultShown = 0,

    // Failed
    WrongKey = -1,
    PlaceHolder2 = -2,
    PlaceHolder3 = -3,
    NotFound = -4,
    UnknownError = -5,
    RemoteNotAccessible = -6,
    Conflict = -7,
    Unauthorized = -8,
    Timeout = -9,
    InvalidInput = -10,
    TooManyRequests = -11
}