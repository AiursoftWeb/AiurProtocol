namespace Aiursoft.AiurProtocol.Models;

/// <summary>
/// Status code for an API response.
/// </summary>
public enum Code
{
    // Success
    /// <summary>
    /// The job is done successfully.
    /// </summary>
    JobDone = 2,
    
    /// <summary>
    /// No action was taken.
    /// </summary>
    NoActionTaken = 1,
    
    /// <summary>
    /// The result is shown.
    /// </summary>
    ResultShown = 0,

    // Failed
    /// <summary>
    /// The key is wrong.
    /// </summary>
    WrongKey = -1,
    
    /// <summary>
    /// Place holder.
    /// </summary>
    PlaceHolder2 = -2,
    
    /// <summary>
    /// Place holder.
    /// </summary>
    PlaceHolder3 = -3,
    
    /// <summary>
    /// The resource was not found.
    /// </summary>
    NotFound = -4,
    
    /// <summary>
    /// An unknown error happened.
    /// </summary>
    UnknownError = -5,
    
    /// <summary>
    /// The remote server is not accessible.
    /// </summary>
    RemoteNotAccessible = -6,
    
    /// <summary>
    /// A conflict happened.
    /// </summary>
    Conflict = -7,
    
    /// <summary>
    /// Unauthorized.
    /// </summary>
    Unauthorized = -8,
    
    /// <summary>
    /// Timed out.
    /// </summary>
    Timeout = -9,
    
    /// <summary>
    /// The input is invalid.
    /// </summary>
    InvalidInput = -10,
    
    /// <summary>
    /// Too many requests.
    /// </summary>
    TooManyRequests = -11
}