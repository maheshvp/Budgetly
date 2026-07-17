using Budgetly.Application.Common.Results;
using Budgetly.Domain.Enums;
using MediatR;

namespace Budgetly.Application.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand(string Name, CategoryType Type) : IRequest<Result<Guid>>;
