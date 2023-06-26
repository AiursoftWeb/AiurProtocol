using Newtonsoft.Json;

namespace Aiursoft.AiurProtocol;

public class AiurPagedCollection<T> : AiurCollection<T>
{
    [Obsolete("This method is only for framework", true)]
    public AiurPagedCollection()
    {
    }

    public AiurPagedCollection(IReadOnlyCollection<T> items) : base(items)
    {
    }

    [JsonProperty(Required = Required.Always)]
    public int TotalCount { get; set; }

    /// <summary>
    ///     Starts from 1.
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    public int CurrentPage { get; set; }

    [JsonProperty(Required = Required.Always)]
    public int CurrentPageSize { get; set; }
}