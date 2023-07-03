using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DemoApiApp.Sdk.Models.ApiAddressModels;

public class ComplicatedRouteAddressModel
{
    [Required] 
    public string? AccessToken { get; set; }

    [Required]
    [FromRoute]
    public string? SiteName { get; set; }

    [FromRoute]
    public string? FolderNames { get; set; }
}