using Budgetly.Application.Common.Results;
using FluentValidation;
using MediatR;

namespace Budgetly.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);
        var failures = validators
            .Select(v => v.Validate(context))
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count == 0)
            return await next(cancellationToken);

        var error = Error.Validation(
            failures[0].PropertyName,
            string.Join("; ", failures.Select(f => f.ErrorMessage)));

        return CreateFailureResult(error);
    }

    private static TResponse CreateFailureResult(Error error)
    {
        var type = typeof(TResponse);

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var valueType = type.GetGenericArguments()[0];
            var method = typeof(Result<>)
                .MakeGenericType(valueType)
                .GetMethod(nameof(Results.Result.Failure), [typeof(Error)])!;
            return (TResponse)method.Invoke(null, [error])!;
        }

        return (TResponse)Results.Result.Failure(error);
    }
}
