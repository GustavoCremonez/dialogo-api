using Dialogo.Domain.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace Dialogo.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
            return new OkResult();

        return ToErrorResult(result.Error);
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result.Value);

        return ToErrorResult(result.Error);
    }

    public static IActionResult ToCreatedResult<T>(this Result<T> result, string? location = null)
    {
        if (result.IsSuccess)
            return new CreatedResult(location ?? string.Empty, result.Value);

        return ToErrorResult(result.Error);
    }

    public static IActionResult ToNoContentResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new NoContentResult();

        return ToErrorResult(result.Error);
    }

    public static IActionResult ToAcceptedResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new AcceptedResult(string.Empty, result.Value);

        return ToErrorResult(result.Error);
    }

    private static IActionResult ToErrorResult(Error error)
    {
        var statusCode = GetStatusCode(error.Type);

        var response = new
        {
            IsSuccess = false,
            Error = new
            {
                error.Code,
                error.Message,
                Type = error.Type.ToString()
            }
        };

        return new ObjectResult(response) { StatusCode = statusCode };
    }

    private static int GetStatusCode(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status400BadRequest
        };
    }
}
