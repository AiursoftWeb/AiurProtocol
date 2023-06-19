namespace Aiursoft.AiurProtocol.Models;

public class AiurValue<T> : AiurResponse
{
    public AiurValue(T value)
    {
        Value = value;
    }

    public T? Value { get; set; }
}