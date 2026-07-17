namespace Budgetly.Application.Common.Results;

public sealed record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static Error NotFound(string resource, object id) =>
        new($"{resource}.NotFound", $"{resource} with id '{id}' was not found.");

    public static Error Validation(string field, string message) =>
        new($"Validation.{field}", message);

    public static Error Conflict(string code, string message) =>
        new(code, message);
}
