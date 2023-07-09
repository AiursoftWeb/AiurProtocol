using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Aiursoft.AiurProtocol;

public class AiurApiEndpoint : AiurApiPayload
{
    public string HostAndRoute { get; set; }

    private AiurApiEndpoint(string hostAndRoute, object param) : base(param)
    {
        HostAndRoute = hostAndRoute;
    }

    public AiurApiEndpoint(string host, string route, object param) : this(
         hostAndRoute: host + "/" + route.TrimStart('/'), 
         param: param)
    {
    }

    public AiurApiEndpoint(string host, string controllerName, string actionName, object param) : this(
        host: host,
        route: $"{WebUtility.UrlEncode(controllerName)}/{WebUtility.UrlEncode(actionName)}",
        param: param)
    {
    }

    public override string ToString()
    {
        var paramsInAddress = GetParamsInAddress();
        var appendPart = "?";
        foreach (var param in Params)
        {
            if (!paramsInAddress.Contains(param.Key) && !string.IsNullOrWhiteSpace(param.Value))
            {
                appendPart = $"{appendPart}{param.Key.ToLower()}={Uri.EscapeDataString(param.Value)}&";
            }
        }

        return InjectParamsToAddress() + appendPart.TrimEnd('?', '&');
    }

    private static string EncodeWithoutPath(string input)
    {
        var encodedString = new StringBuilder();

        foreach (char c in input)
        {
            if (c == '/')
            {
                encodedString.Append(c);
            }
            else
            {
                encodedString.Append(Uri.EscapeDataString(c.ToString()));
            }
        }

        return encodedString.ToString();
    }

    public virtual string[] GetParamsInAddress()
    {
        var matches = Regex.Matches(HostAndRoute, @"\{([^{}]+)\}");
        return matches.Select(m => m.Groups[1].Value).Select(v => v.TrimStart('*')).ToArray();
    }

    public virtual string InjectParamsToAddress()
    {
        return Regex.Replace(HostAndRoute, @"\{([^{}]+)\}", match =>
        {
            var key = match.Groups[1].Value;
            if (key.StartsWith("*"))
            {
                key = key.TrimStart('*');
                if (Params.TryGetValue(key, out var actualValue))
                {
                    return EncodeWithoutPath(actualValue);
                }
                else
                {
                    throw new InvalidOperationException($"Unknown variable name '{key}' in template: '{HostAndRoute}'!");
                }
            }
            else
            {
                if (Params.TryGetValue(key, out var actualValue))
                {
                    return Uri.EscapeDataString(actualValue);
                }
                else
                {
                    throw new InvalidOperationException($"Unknown variable name '{key}' in template: '{HostAndRoute}'!");
                }
            }
        });
    }
}