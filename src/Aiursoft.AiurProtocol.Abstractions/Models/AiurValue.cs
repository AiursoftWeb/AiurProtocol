using Newtonsoft.Json;

namespace Aiursoft.AiurProtocol.Models;

public class AiurValue<T> : AiurResponse
{
    public AiurValue(T value)
    {
        Value = value;
    }

    [JsonProperty(Required = Required.Always)]
    public T? Value { get; set; }
}