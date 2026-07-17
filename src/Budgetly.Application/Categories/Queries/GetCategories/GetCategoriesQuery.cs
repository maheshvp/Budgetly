using Budgetly.Application.Categories.Dtos;
using Budgetly.Application.Common.Results;
using MediatR;

namespace Budgetly.Application.Categories.Queries.GetCategories;

public sealed record GetCategoriesQuery : IRequest<Result<IReadOnlyList<CategoryDto>>>;
