using Dialogo.Domain.Shared.Results;

namespace Dialogo.Domain.Shared.Exceptions;

public class ValidationException : Exception
{
    public Error[] Errors { get; }

    public ValidationException(Error[] errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }
}
