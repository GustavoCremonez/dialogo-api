using System.Text.Json;
using Dialogo.Domain.Shared.Exceptions;
using Dialogo.Domain.Shared.Results;

namespace Dialogo.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, error) = GetErrorResponse(exception);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

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

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }

    private static (int StatusCode, Error Error) GetErrorResponse(Exception exception)
    {
        return exception switch
        {
            ValidationException validationEx =>
                (StatusCodes.Status400BadRequest, validationEx.Errors.FirstOrDefault() ?? Error.Validation("Validation", validationEx.Message)),

            BadRequestException badRequestEx =>
                (StatusCodes.Status400BadRequest, Error.Failure("BadRequest", badRequestEx.Message)),

            NotFoundException notFoundEx =>
                (StatusCodes.Status404NotFound, Error.NotFound("NotFound", notFoundEx.Message)),

            UnauthorizedException unauthorizedEx =>
                (StatusCodes.Status401Unauthorized, Error.Unauthorized("Unauthorized", unauthorizedEx.Message)),

            ForbiddenException forbiddenEx =>
                (StatusCodes.Status403Forbidden, Error.Forbidden("Forbidden", forbiddenEx.Message)),

            ConflictException conflictEx =>
                (StatusCodes.Status409Conflict, Error.Conflict("Conflict", conflictEx.Message)),

            DomainException domainEx =>
                (StatusCodes.Status400BadRequest, Error.Failure("DomainError", domainEx.Message)),

            _ =>
                (StatusCodes.Status500InternalServerError, Error.Failure("InternalServerError", "An unexpected error occurred."))
        };
    }
}
