using Aiursoft.AiurProtocol;
using Aiursoft.AiurProtocol.Models;

namespace DemoApiApp.Sdk.Models.ApiAddressModels;

public class QueryNumberAddressModel : Pager
{
    public string? Question { get; set; }
}