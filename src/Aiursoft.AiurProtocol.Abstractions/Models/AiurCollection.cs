using Newtonsoft.Json;

namespace Aiursoft.AiurProtocol;

public class AiurCollection<T> : AiurResponse
{
    [Obsolete("This method is only for framework", true)]
    public AiurCollection()
    {
    }

    public AiurCollection(IReadOnlyCollection<T> items)
    {
        Items = items;
    }

    [JsonProperty(Required = Required.Always)]
    public IReadOnlyCollection<T>? Items { get; set; }
}