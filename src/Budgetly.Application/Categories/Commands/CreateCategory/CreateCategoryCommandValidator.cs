using FluentValidation;

namespace Budgetly.Application.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Category name is required.")
            .MaximumLength(50)
            .WithMessage("Category name must not exceed 50 characters.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Type must be a valid CategoryType.");
    }
}
