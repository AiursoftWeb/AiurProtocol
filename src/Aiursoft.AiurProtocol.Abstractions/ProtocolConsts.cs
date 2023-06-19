using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Aiursoft.AiurProtocol.Abstractions.Configuration;

public static class ProtocolConsts
{
    public static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
    {
        DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };
}