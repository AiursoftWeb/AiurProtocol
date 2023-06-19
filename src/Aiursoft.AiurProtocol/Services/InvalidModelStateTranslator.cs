using Aiursoft.AiurProtocol.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Aiursoft.AiurProtocol.Services;

internal static class InvalidModelStateTranslator
{
    internal static AiurCollection<string> GetInvalidModelStateErrorResponse(
        ModelStateDictionary modelState)
    {
        var list = (from value in modelState from error in value.Value.Errors select error.ErrorMessage).ToList();
        var arg = new AiurCollection<string>(list)
        {
            Code = Code.InvalidInput,
            Message = $"Multiple errors were found in the API input. ({list.Count} errors)"
        };
        return arg;
    }
}