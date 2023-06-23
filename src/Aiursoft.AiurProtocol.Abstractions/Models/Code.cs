namespace Aiursoft.AiurProtocol.Models;

public enum Code
{
    NoActionNeeded = 1,
    Success = 0,
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