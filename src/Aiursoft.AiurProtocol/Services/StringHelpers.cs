using Aiursoft.AiurProtocol.Models;
using Newtonsoft.Json;

namespace Aiursoft.AiurProtocol.Services;

public static class StringHelpers
{
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
                result = JsonConvert.DeserializeObject<AiurResponse>(strInput, ProtocolSettings.JsonSettings);
                return result != null;
            }
            catch
            {
                // Suppress and return false.
            }
        }

        return false;
    }
}
