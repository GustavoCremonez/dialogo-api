using Dialogo.Domain.Shared.Results;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dialogo.Api.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;
    private static readonly BindingSource[] ValidatableSources =
    [
        BindingSource.Form,
        BindingSource.Query,
        BindingSource.Body
    ];

    public ValidationFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var parameters = context.ActionDescriptor.Parameters
            .Where(p => p.BindingInfo is not null
                && p.ParameterType.IsClass
                && ValidatableSources.Contains(p.BindingInfo.BindingSource))
            .ToList();

        var errors = new List<Error>();

        foreach (ParameterDescriptor parameter in parameters)
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);
            if (_serviceProvider.GetService(validatorType) is not IValidator validator)
                continue;

            var subject = context.ActionArguments[parameter.Name];
            if (subject is null)
                continue;

            var validationContext = new ValidationContext<object>(subject);
            var result = await validator.ValidateAsync(validationContext, context.HttpContext.RequestAborted);

            if (!result.IsValid)
            {
                errors.AddRange(result.Errors.Select(e => Error.Validation(e.PropertyName, e.ErrorMessage)));
            }
        }

        if (errors.Count > 0)
        {
            var response = new
            {
                IsSuccess = false,
                Errors = errors.Select(e => new { e.Code, e.Message, Type = e.Type.ToString() })
            };

            context.Result = new BadRequestObjectResult(response);
            return;
        }

        await next();
    }
}
