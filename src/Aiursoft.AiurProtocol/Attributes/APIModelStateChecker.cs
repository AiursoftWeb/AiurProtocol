using Aiursoft.AiurProtocol.Exceptions;
using Aiursoft.AiurProtocol.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aiursoft.AiurProtocol.Attributes;

/// <summary>
///     This attribute will not throw any exception but will reject any invalid request directly with AiurCollection with
///     error messages.
///     Strongly suggest adding this attribute to all API controllers.
/// </summary>
public class ApiModelStateChecker : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var response = InvalidModelStateTranslator.GetInvalidModelStateErrorResponse(context.ModelState);
            throw new AiurServerException(response);
        }

        base.OnActionExecuting(context);
    }
}