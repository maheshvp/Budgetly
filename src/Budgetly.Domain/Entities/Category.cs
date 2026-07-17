using Budgetly.Domain.Enums;

namespace Budgetly.Domain.Entities;

public sealed class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public CategoryType Type { get; private set; }

    private Category() { }

    public static Category Create(string name, CategoryType type)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name is required.", nameof(name));

        if (name.Length > 50)
            throw new ArgumentException("Category name must not exceed 50 characters.", nameof(name));

        return new Category
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Type = type
        };
    }
}
