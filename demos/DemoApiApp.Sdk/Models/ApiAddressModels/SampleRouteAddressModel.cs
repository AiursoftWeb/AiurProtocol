using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DemoApiApp.Sdk.Models.ApiAddressModels;

public class SampleRouteAddressModel
{
    [FromRoute]
    [Required]
    public int Id { get; set; }
}