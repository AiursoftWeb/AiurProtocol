using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Aiursoft.AiurProtocol;

public static class ProtocolSettings
{
    public static readonly JsonSerializerSettings JsonSettings = new()
    {
        DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };
}