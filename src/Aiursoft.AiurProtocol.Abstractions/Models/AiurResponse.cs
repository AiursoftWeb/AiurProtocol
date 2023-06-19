namespace Aiursoft.AiurProtocol.Models;

public class AiurResponse
{
    public virtual ErrorType Code { get; set; }
    public virtual string? Message { get; set; }
}