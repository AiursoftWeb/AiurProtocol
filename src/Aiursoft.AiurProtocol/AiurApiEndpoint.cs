﻿using System.Net;

namespace Aiursoft.AiurProtocol;

public class AiurApiEndpoint : AiurApiPayload
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
