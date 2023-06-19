using Aiursoft.AiurProtocol.Abstractions.Configuration;
using Aiursoft.AiurProtocol.Models;
using Newtonsoft.Json;
using JsonException = System.Text.Json.JsonException;

namespace Aiursoft.AiurProtocol.Services;

public static class StringHelpers
{
    public static string SafeTakeFirst(this string source, int count)
    {
        return source.Length <= count ? source : string.Concat(source.AsSpan(0, count - 3), "...");
    }
    
    public static bool IsValidResponse(this string strInput, out AiurResponse? result)
    {
        result = default;
        if (string.IsNullOrWhiteSpace(strInput))
        {
            return false;
        }

        strInput = strInput.Trim();
        if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object.
            (strInput.StartsWith("\"") && strInput.EndsWith("\"")) || // For string.
            (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array.
        {
            try
            {
                // throw exception for illegal json format.
                result = JsonConvert.DeserializeObject<AiurResponse>(strInput, ProtocolConsts.JsonSettings); 
                return result != null;
            }
            catch
            {
                // Suppress and return false.
            }

            return false;
        }

        return false;
    }
}