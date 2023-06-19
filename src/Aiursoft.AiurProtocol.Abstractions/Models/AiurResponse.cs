﻿using Newtonsoft.Json;

namespace Aiursoft.AiurProtocol.Models;

public class AiurResponse
{
    [JsonProperty(Required = Required.Always)]
    public ErrorType Code { get; set; }
    
    [JsonProperty(Required = Required.Always)]
    public string? Message { get; set; }
}