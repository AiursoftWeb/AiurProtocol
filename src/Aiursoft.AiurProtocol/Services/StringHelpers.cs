using Newtonsoft.Json;
using JsonException = System.Text.Json.JsonException;

namespace Aiursoft.AiurProtocol.Services;

public static class StringHelpers
{
    public static string SafeTakeFirst(this string source, int count)
    {
        return source.Length <= count ? source : string.Concat(source.AsSpan(0, count - 3), "...");
    }
    
    public static bool IsValidJson<T>(this string strInput, out T? result)
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
                result = JsonConvert.DeserializeObject<T>(strInput); // throw exception for illegal json format.
                return true;
            }
            catch (JsonException)
            {
            }

            return false;
        }

        return false;
    }
}