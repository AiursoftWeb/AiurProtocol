using Newtonsoft.Json;
using System.Reflection;

namespace Aiursoft.AiurProtocol.Models;

public class AiurResponse
{
    [JsonProperty(Required = Required.Always)]
    public Code Code { get; set; }

    [JsonProperty(Required = Required.Always)]
    public string? Message { get; set; }

    public Version ProtocolVersion
    {
        get;
        [Obsolete(message: "The set method is only for Json deserializer!")]
        set;
    } = Assembly.GetExecutingAssembly()?.GetName()?.Version ?? new Version();
}