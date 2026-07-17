using Budgetly.Domain.Entities;
using Budgetly.Domain.Enums;

namespace Budgetly.Application.Categories.Dtos;

public sealed record CategoryDto(Guid Id, string Name, CategoryType Type)
{
    public static CategoryDto FromEntity(Category category) =>
        new(category.Id, category.Name, category.Type);
}
