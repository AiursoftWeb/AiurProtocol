using Newtonsoft.Json;

namespace Aiursoft.AiurProtocol;

public class AiurResponse
{
    [JsonProperty(Required = Required.Always)]
    public Code Code { get; set; }

    [JsonProperty(Required = Required.Always)]
    public string? Message { get; set; }

    public Version? ProtocolVersion { get; set; }
}