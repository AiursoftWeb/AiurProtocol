# AiurProtocol

[![MIT licensed](https://img.shields.io/badge/license-MIT-blue.svg)](https://gitlab.aiursoft.cn/aiursoft/aiurprotocol/-/blob/master/LICENSE)
[![Pipeline stat](https://gitlab.aiursoft.cn/aiursoft/aiurprotocol/badges/master/pipeline.svg)](https://gitlab.aiursoft.cn/aiursoft/aiurprotocol/-/pipelines)
[![Test Coverage](https://gitlab.aiursoft.cn/aiursoft/aiurprotocol/badges/master/coverage.svg)](https://gitlab.aiursoft.cn/aiursoft/aiurprotocol/-/pipelines)
[![NuGet version (Aiursoft.AiurProtocol)](https://img.shields.io/nuget/v/Aiursoft.AiurProtocol.svg)](https://www.nuget.org/packages/Aiursoft.AiurProtocol/)

AiurProtocol defines an API programming practice to easily build a RESTful API. It simplifies the process of 

* Auto HTTP status code translation
* Auto error handling
* Auto input model validation
* Client side API validation

## Why this project?

API development is a challenging task that requires handling various aspects such as HTTP status codes, error handling, input validation, documentation writing, and log checking. However, this project aims to simplify the API development process by providing a unified best practice approach. By following this approach, developers can efficiently handle HTTP status codes, error handling, input validation, documentation writing, and log checking. This project's goal is to save time and effort, allowing developers to focus more on developing new features.

## Installation

Run the folloing command to install `Aiursoft.AiurProtocol` to your ASP.NET Core project from [nuget.org](https://www.nuget.org/packages/Aiursoft.AiurProtocol/):

```bash
dotnet add package Aiursoft.AiurProtocol
```

## How to use on Server

Now you can go to your Controller and return the protocol!

```csharp
using Aiursoft.AiurProtocol;

public class HomeController : ControllerBase
{
    public IActionResult Index()
    {
        return this.Protocol(Code.Success, "Welcome to this API project!");
    }
}
```

## How to use it to build an SDK

Now you need to write an SDK for your API.

After creating a new class library project, add the dependencies:

```xml
<Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <TargetFramework>net6.0</TargetFramework>
        <ImplicitUsings>enable</ImplicitUsings>
        <Nullable>enable</Nullable>
    </PropertyGroup>
    <ItemGroup>
        <PackageReference Include="Microsoft.Extensions.Options.ConfigurationExtensions" Version="6.0.0" />
        <PackageReference Include="Aiursoft.AiurProtocol" Version="6.0.0" />
    </ItemGroup>
</Project>
```

write the following method:

```csharp
using Aiursoft.AiurProtocol;
using Microsoft.Extensions.DependencyInjection;

public static IServiceCollection AddDemoService(this IServiceCollection services, string endPointUrl)
{
    services.AddAiurProtocolClient();
    services.Configure<DemoServerConfig>(options => options.Instance = endPointUrl);
    services.AddScoped<DemoAccess>();
    return services;
}

public class DemoServerConfig
{
    public string Instance { get; set; } = string.Empty;
}

public class DemoAccess
{
    private readonly AiurProtocolClient _http;
    private readonly DemoServerConfig _demoServerLocator;

    public DemoAccess(
        AiurProtocolClient http,
        IOptions<DemoServerConfig> demoServerLocator)
    {
        _http = http;
        _demoServerLocator = demoServerLocator.Value;
    }

    public async Task<AiurResponse> IndexAsync()
    {
        var url = new AiurApiEndpoint(_demoServerLocator.Instance);
        var result = await _http.Get<AiurResponse>(url);
        return result;
    }
}
```

## How to use my new SDK

Now you can write a new class lib to use your new SDK to call your server!

```csharp
// To get your SDK:
var services = new ServiceCollection();
services.AddDemoService(endpointUrl);
var serviceProvider = services.BuildServiceProvider();
var sdk = serviceProvider.GetRequiredService<DemoAccess>();

// To use your SDK:
var result = await sdk?.IndexAsync()!;
```

That's it! It will use your SDK to generate a new call to your server, and the result is right at your hand!

## Advanced usage

* API Design
  * [Call with HTTP parameter](./inop.md)
  * [Call with HTTP Post(Json)](./inop.md)
  * [Call with HTTP Post(Form)](./inop.md)
* Server Programming
  * [Response complicated values](./inop.md)
  * [Result paging](./inop.md)
  * [Return expected error](./inop.md)
  * [Unexpected error](./inop.md)
  * [Input valid state checking](./inop.md)

## Future features

It will support the following features in the future:

* Api rate limit
* Api version control
* Api documentation
* Api request logging and report

## How to contribute

There are many ways to contribute to the project: logging bugs, submitting pull requests, reporting issues, and creating suggestions.

Even if you with push rights on the repository, you should create a personal fork and create feature branches there when you need them. This keeps the main repository clean and your workflow cruft out of sight.

We're also interested in your feedback on the future of this project. You can submit a suggestion or feature request through the issue tracker. To make this process more effective, we're asking that these include more information to help define them more clearly.