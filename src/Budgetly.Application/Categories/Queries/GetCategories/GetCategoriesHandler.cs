using Budgetly.Application.Categories.Dtos;
using Budgetly.Application.Common.Interfaces;
using Budgetly.Application.Common.Results;
using MediatR;

namespace Budgetly.Application.Categories.Queries.GetCategories;

public sealed class GetCategoriesHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<GetCategoriesQuery, Result<IReadOnlyList<CategoryDto>>>
{
    public async Task<Result<IReadOnlyList<CategoryDto>>> Handle(
        GetCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var categories = await categoryRepository.GetAllAsync(cancellationToken);
        var dtos = categories.Select(CategoryDto.FromEntity).ToList();

        return Result.Success<IReadOnlyList<CategoryDto>>(dtos);
    }
}
