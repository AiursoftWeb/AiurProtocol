using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Aiursoft.AiurProtocol;

public class AiurApiPayload
{
    public object? Param { get; set; }
    public Dictionary<string, string> Params { get; } = new();

    public AiurApiPayload()
    {
    }
    
    public AiurApiPayload(object param)
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