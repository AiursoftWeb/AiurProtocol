using System.Globalization;

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
            var propName = prop.Name;
            var propValue = prop.GetValue(param)?.ToString() ?? string.Empty;
            // Keep honoring ASP.NET Core's IModelNameProvider attributes when the client is
            // used by a web app, without making the portable client library depend on the
            // ASP.NET Core shared framework (which is unavailable on Android).
            var modelNameAttribute = prop.GetCustomAttributes(true).FirstOrDefault(attribute =>
                attribute.GetType().GetInterfaces().Any(contract =>
                    contract.FullName == "Microsoft.AspNetCore.Mvc.ModelBinding.IModelNameProvider"));
            var customName = modelNameAttribute?.GetType().GetProperty("Name")?.GetValue(modelNameAttribute) as string;
            if (!string.IsNullOrEmpty(customName))
            {
                propName = customName;
            }

            if (prop.PropertyType == typeof(DateTime))
            {
                if (prop.GetValue(param) is DateTime time)
                {
                    propValue = time.ToString("o", CultureInfo.InvariantCulture);
                }
            }
            Params.Add(propName, propValue);
        }
    }
}
