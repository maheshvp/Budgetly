using FluentValidation;

namespace Budgetly.Application.Transactions.Commands.CreateTransaction;

public sealed class CreateTransactionCommandValidator
    : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0m)
            .WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .Matches(@"^[A-Za-z]{3}$")
            .WithMessage("Currency must be a 3-letter ISO code.");

        RuleFor(x => x.Date)
            .NotEqual(default(DateOnly))
            .WithMessage("Date is required.");

        RuleFor(x => x.Note)
            .MaximumLength(250)
            .WithMessage("Note must not exceed 250 characters.")
            .When(x => x.Note is not null);
    }
}
