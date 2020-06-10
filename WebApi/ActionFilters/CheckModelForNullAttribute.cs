using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Scar.Common.WebApi.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CheckModelForNullAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));
            var requiredParametersWithNullValue = context.ActionDescriptor.Parameters.Cast<ControllerParameterDescriptor>()
                .Where(i => !i.ParameterInfo.IsOptional && (context.ActionArguments.Single(j => j.Key == i.ParameterInfo.Name).Value == null))
                .ToArray();

            if (requiredParametersWithNullValue.Length == 0)
            {
                return;
            }

            var message = $"{string.Join(", ", requiredParametersWithNullValue.Select(i => i.ParameterInfo.Name))} cannot be null";
            context.Result = new BadRequestObjectResult(message);
        }
    }
}
