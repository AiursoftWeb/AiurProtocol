# AiurProtocol

[![MIT licensed](https://img.shields.io/badge/license-MIT-blue.svg)](https://gitlab.aiursoft.cn/aiursoft/aiurprotocol/-/blob/master/LICENSE)
[![Pipeline stat](https://gitlab.aiursoft.cn/aiursoft/aiurprotocol/badges/master/pipeline.svg)](https://gitlab.aiursoft.cn/aiursoft/aiurprotocol/-/pipelines)
[![Test Coverage](https://gitlab.aiursoft.cn/aiursoft/aiurprotocol/badges/master/coverage.svg)](https://gitlab.aiursoft.cn/aiursoft/aiurprotocol/-/pipelines)
[![NuGet version (Aiursoft.AiurProtocol)](https://img.shields.io/nuget/v/Aiursoft.AiurProtocol.svg)](https://www.nuget.org/packages/Aiursoft.AiurProtocol/)

AiurProtocol defines an API programming practice to easily build a RESTful API. It simplifies the process of 

* Auto HTTP status code translation
* Auto error handling
* Auto input model validation

And will support the following features in the future:

* Client side format check
* Api Localization
* Api rate limit
* Api version control
* Api documentation
* Api authorization
* Api request log
* Api request cache

## How to Install

## How to use on Server

```csharp
return this.Protocol();

throw new AiurApiModelException();
```

## How to contribute

There are many ways to contribute to the project: logging bugs, submitting pull requests, reporting issues, and creating suggestions.

Even if you with push rights on the repository, you should create a personal fork and create feature branches there when you need them. This keeps the main repository clean and your workflow cruft out of sight.

We're also interested in your feedback on the future of this project. You can submit a suggestion or feature request through the issue tracker. To make this process more effective, we're asking that these include more information to help define them more clearly.