using Aiursoft.AiurProtocol;
using Aiursoft.AiurProtocol.Models;

namespace DemoApiApp.Sdk.Models.ApiViewModels;

public class RegisterViewModel : AiurResponse
{
    public string? UserId { get; set; }
}