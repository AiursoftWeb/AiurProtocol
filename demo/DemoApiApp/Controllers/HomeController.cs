using Aiursoft.AiurProtocol;
using Aiursoft.AiurProtocol.Models;
using DemoApiApp.Sdk.Models.ApiAddressModels;
using Microsoft.AspNetCore.Mvc;

namespace DemoApiApp.Controllers;

public class HomeController : ControllerBase
{
    public IActionResult Index()
    {
        return this.Protocol(ErrorType.Success, "Welcome to this API project!");
    }

    public IActionResult InvalidResponseShouldNotSuccess()
    {
        return Ok(new { message = "This is not a valid Protocol response." });
    }

    public IActionResult GetANumber()
    {
        return this.Protocol(ErrorType.Success, "Got your value!", value: 123);
    }
    
    public IActionResult QuerySomething([FromQuery]string question)
    {
        var items = Fibonacci()
            .Where(i => i.ToString().EndsWith(question))
            .Take(10)
            .ToList();
        return this.Protocol(ErrorType.Success, "Got your value!", items);
    }

    public IActionResult GetFibonacciFirst10()
    {
        var items = Fibonacci().Take(10).ToList();
        return this.Protocol(ErrorType.Success, "Got your value!", items);
    }

    [HttpPost]
    public IActionResult RegisterForm([FromForm] RegisterAddressModel model)
    {
        return this.Protocol(new RegisterViewModel
        {
            Code = ErrorType.Success,
            Message = "Registered.",
            UserId = "your-id-" + model.Name
        });
    }
    
    [HttpPost]
    public IActionResult RegisterJson([FromBody] RegisterAddressModel model)
    {
        return this.Protocol(new RegisterViewModel
        {
            Code = ErrorType.Success,
            Message = "Registered.",
            UserId = "your-id-" + model.Name
        });
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