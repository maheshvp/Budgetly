using Budgetly.Domain.Entities;
using Budgetly.Domain.Enums;

namespace Budgetly.Domain.Tests.Entities;

public sealed class CategoryTests
{
    [Fact]
    public void Create_ValidInputs_ReturnsCategoryWithCorrectProperties()
    {
        var category = Category.Create("Groceries", CategoryType.Expense);

        category.Id.ShouldNotBe(Guid.Empty);
        category.Name.ShouldBe("Groceries");
        category.Type.ShouldBe(CategoryType.Expense);
    }

    [Fact]
    public void Create_IncomeType_SetsTypeCorrectly()
    {
        var category = Category.Create("Salary", CategoryType.Income);

        category.Type.ShouldBe(CategoryType.Income);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_EmptyOrWhitespaceName_ThrowsArgumentException(string badName)
    {
        Should.Throw<ArgumentException>(() => Category.Create(badName, CategoryType.Expense));
    }

    [Fact]
    public void Create_NameExceeds50Chars_ThrowsArgumentException()
    {
        var longName = new string('X', 51);
        Should.Throw<ArgumentException>(() => Category.Create(longName, CategoryType.Expense));
    }

    [Fact]
    public void Create_NameWithLeadingAndTrailingSpaces_TrimsName()
    {
        var category = Category.Create("  Groceries  ", CategoryType.Expense);

        category.Name.ShouldBe("Groceries");
    }

    [Fact]
    public void Create_EachCallProducesDifferentId()
    {
        var c1 = Category.Create("A", CategoryType.Income);
        var c2 = Category.Create("B", CategoryType.Expense);

        c1.Id.ShouldNotBe(c2.Id);
    }
}
