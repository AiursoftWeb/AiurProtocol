using System.ComponentModel.DataAnnotations;

namespace DemoApiApp.Sdk.Models.ApiAddressModels;

public class RegisterAddressModel
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Password { get; set; }
}