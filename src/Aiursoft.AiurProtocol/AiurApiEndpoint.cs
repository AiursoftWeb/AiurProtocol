using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Aiursoft.AiurProtocol;

public class ApiPayload
{
    public object? Param { get; set; }
    public Dictionary<string, string> Params { get; } = new();

    public ApiPayload()
    {
    }
    
    public ApiPayload(object param)
    {
        Param = param;
        var t = param.GetType();
        foreach (var prop in t.GetProperties())
        {
            if (prop.GetValue(param) == null)
            {
                continue;
            }

            var propName = prop.Name;
            var propValue = prop.GetValue(param)?.ToString();
            var fromQuery = prop.GetCustomAttributes(typeof(IModelNameProvider), true).FirstOrDefault();
            if (fromQuery is IModelNameProvider nameProvider && nameProvider.Name != null)
            {
                propName = nameProvider.Name;
            }

            if (prop.PropertyType == typeof(DateTime))
            {
                if (prop.GetValue(param) is DateTime time)
                {
                    propValue = time.ToString("o", CultureInfo.InvariantCulture);
                }
            }

            if (!string.IsNullOrWhiteSpace(propValue))
            {
                Params.Add(propName, propValue);
            }
        }
    }
}

public class AiurApiEndpoint : ApiPayload
{
    public string Address { get; set; }

    public AiurApiEndpoint(string address)
    {
        Address = address;
    }

    public AiurApiEndpoint(string address, object param) : base(param)
    {
        Address = address;
    }

    public AiurApiEndpoint(string host, string path, object param) : this(host + path, param)
    {
    }

    public AiurApiEndpoint(string host, string controllerName, string actionName, object param) : this(host,
        $"/{WebUtility.UrlEncode(controllerName)}/{WebUtility.UrlEncode(actionName)}", param)
    {
    }

    public override string ToString()
    {
        var appendPart = Params.Aggregate("?", (c, p) =>
            $"{c}{p.Key.ToLower()}={Uri.EscapeDataString(p.Value)}&");
        return Address + appendPart.TrimEnd('?', '&');
    }
}