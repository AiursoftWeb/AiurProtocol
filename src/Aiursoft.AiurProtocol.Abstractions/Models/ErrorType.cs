﻿namespace Aiursoft.AiurProtocol.Models;

public enum ErrorType
{
    Success = 0,
    WrongKey = -1,
    InsufficientPermissions = -2,
    Gone = -3,
    NotFound = -4,
    UnknownError = -5,
    RemoteNotAccessible = -6,
    Conflict = -7,
    Unauthorized = -8,
    Timeout = -9,
    InvalidInput = -10,
    TooManyRequests = -11
}