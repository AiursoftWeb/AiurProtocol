using System.Diagnostics.CodeAnalysis;
using Aiursoft.AiurProtocol;
using Aiursoft.AiurProtocol.Attributes;
using Aiursoft.AiurProtocol.Exceptions;
using Aiursoft.AiurProtocol.Models;
using DemoApiApp.Sdk.Models.ApiAddressModels;
using DemoApiApp.Sdk.Models.ApiViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DemoApiApp.Controllers;

[ApiExceptionHandler(
    PassthroughRemoteErrors = true, 
    PassthroughAiurServerException = true)]
[ApiModelStateChecker]
public class HomeController : ControllerBase
{
    public IActionResult Index()
    {
        return this.Protocol(Code.Success, "Welcome to this API project!");
    }

    public IActionResult InvalidResponseShouldNotSuccess()
    {
        return BadRequest(new { message = "This is not a valid Protocol response." });
    }

    public IActionResult GetANumber()
    {
        return this.Protocol(Code.NoActionNeeded, "Got your value!", value: 123);
    }

    public IActionResult QuerySomething([FromQuery] string question)
    {
        var items = Fibonacci()
            .Where(i => i.ToString().EndsWith(question))
            .Take(10)
            .ToList();
        return this.Protocol(Code.Success, "Got your value!", items);
    }
    
    public async Task<IActionResult> QuerySomethingPaged([FromQuery]QueryNumberAddressModel model)
    {
        var database = Fibonacci()
            .Take(30)
            .AsQueryable();
        var items = database
            .Where(i => i.ToString().EndsWith(model.Question ?? string.Empty))
            .AsQueryable()
            .OrderBy(i => i);
        return await this.Protocol(Code.Success, "Got your value!", items, model);
    }

    public IActionResult GetFibonacciFirst10()
    {
        var items = Fibonacci().Take(10).ToList();
        return this.Protocol(Code.Success, "Got your value!", items);
    }

    [HttpPost]
    public IActionResult RegisterForm([FromForm] RegisterAddressModel model)
    {
        return this.Protocol(new RegisterViewModel
        {
            Code = Code.Success,
            Message = "Registered.",
            UserId = "your-id-" + model.Name
        });
    }

    [HttpPost]
    public IActionResult RegisterJson([FromBody] RegisterAddressModel model)
    {
        return this.Protocol(new RegisterViewModel
        {
            Code = Code.Success,
            Message = "Registered.",
            UserId = "your-id-" + model.Name
        });
    }

    public IActionResult CrashKnown()
    {
        throw new AiurServerException(Code.Conflict, "Known error");
    }

    [ExcludeFromCodeCoverage]
    public IActionResult CrashUnknown()
    {
        var one = 1;
        // ReSharper disable once IntDivisionByZero
        _ = 3 / (1 - one);
        return Ok();
    }

    private IEnumerable<int> Fibonacci()
    {
        int current = 1, next = 1;

        while (true)
        {
            yield return current;
            next = current + (current = next);
        }
        // ReSharper disable once IteratorNeverReturns
    }
}