using System.Net;
using Aiursoft.AiurProtocol.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Aiursoft.AiurProtocol.Services;

internal static class InvalidModelStateTranslator
{
    internal static (JsonResult result, HttpStatusCode code) GetInvalidModelStateErrorResponse(
        ModelStateDictionary modelState)
    {
        var list = (from value in modelState from error in value.Value.Errors select error.ErrorMessage).ToList();
        var arg = new AiurCollection<string>(list)
        {
            Code = ErrorType.InvalidInput,
            Message = "Your input contains several errors!"
        };
        var code = arg.ConvertToHttpStatusCode();
        return (new JsonResult(arg), code);
    }
}