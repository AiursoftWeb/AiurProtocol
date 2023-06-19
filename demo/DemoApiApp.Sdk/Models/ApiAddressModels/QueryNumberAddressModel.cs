using Aiursoft.AiurProtocol.Interfaces;

namespace DemoApiApp.Sdk.Models.ApiAddressModels;

public class QueryNumberAddressModel : IPager
{
    public string? Question { get; set; }
    public int PageSize { get; set; }
    
    /// <summary>
    /// Start with 1
    /// </summary>
    public int PageNumber { get; set; }
}