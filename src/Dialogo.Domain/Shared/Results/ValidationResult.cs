namespace Dialogo.Domain.Shared.Results;

public sealed class ValidationResult : Result
{
    public Error[] Errors { get; }

    private ValidationResult(Error[] errors)
        : base(false, errors.Length > 0 ? errors[0] : Error.Validation("Validation", "One or more validation errors occurred."))
    {
        Errors = errors;
    }

    public static ValidationResult WithErrors(Error[] errors) => new(errors);
}

public sealed class ValidationResult<T> : Result<T>
{
    public Error[] Errors { get; }

    private ValidationResult(Error[] errors)
        : base(false, default, errors.Length > 0 ? errors[0] : Error.Validation("Validation", "One or more validation errors occurred."))
    {
        Errors = errors;
    }

    public static ValidationResult<T> WithErrors(Error[] errors) => new(errors);
}
