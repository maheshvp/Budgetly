using FluentValidation;

namespace Budgetly.Application.Accounts.Commands.CreateAccount;

public sealed class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Account name is required.")
            .MaximumLength(100)
            .WithMessage("Account name must not exceed 100 characters.");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .Matches(@"^[A-Za-z]{3}$")
            .WithMessage("Currency must be a 3-letter ISO code.");
    }
}
