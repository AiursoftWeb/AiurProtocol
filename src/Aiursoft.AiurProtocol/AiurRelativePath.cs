using System.Net;

namespace Aiursoft.AiurProtocol;


public class AiurRelativePath : AiurApiPayload
{
    public string Address { get; set; }

    public AiurRelativePath(string address)
    {
        Address = address;
    }

    public AiurRelativePath(string address, object param) : base(param)
    {
        Address = address;
    }

    public AiurRelativePath(string controllerName, string actionName, object param) : this(
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