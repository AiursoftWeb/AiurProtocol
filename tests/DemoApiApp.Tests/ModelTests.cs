using System.ComponentModel.DataAnnotations;
using Aiursoft.AiurProtocol;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoApiApp.Tests;

public class FinishAuthInfo
{
    [Required]
    [FromQuery(Name = "appid")]
    public string? AppId { get; set; }

    [Url]
    [Required]
    [FromQuery(Name = "redirect_uri")]
    public string? RedirectUri { get; set; }

    [FromQuery(Name = "state")] public string? State { get; set; }
}

public class ViewContentAddressModel
{
    [Required] public string? AccessToken { get; set; }

    [Required] [FromRoute] public string? SiteName { get; set; }

    [FromRoute] public string? FolderNames { get; set; }
}

[TestClass]
public class ModelTests
{
    [TestMethod]
    public void TestParam()
    {
        var result = new AiurApiEndpoint(string.Empty, "OAuth", "SecondAuth", new FinishAuthInfo
        {
            AppId = "appId",
            RedirectUri = "redirect_uri"
        }).ToString();
        Assert.AreEqual("/OAuth/SecondAuth?appid=appId&redirect_uri=redirect_uri", result);
    }
    
    [TestMethod]
    public void TestRouteWithEmptyParam()
    {
        var url = new AiurApiEndpoint("https://www.google.com",
            "/Folders/ViewContent/{SiteName}/{**FolderNames}", new ViewContentAddressModel
            {
                SiteName = "siteName",
                FolderNames = string.Empty,
                AccessToken = "accessToken"
            });
        var result = url.ToString();
        Assert.AreEqual(
            "https://www.google.com/Folders/ViewContent/siteName/?accesstoken=accessToken",
            result);
    }
}