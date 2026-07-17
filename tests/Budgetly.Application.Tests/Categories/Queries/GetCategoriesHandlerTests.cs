using Budgetly.Application.Categories.Queries.GetCategories;
using Budgetly.Application.Common.Interfaces;
using Budgetly.Domain.Entities;
using Budgetly.Domain.Enums;

namespace Budgetly.Application.Tests.Categories.Queries;

public sealed class GetCategoriesHandlerTests
{
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();

    private GetCategoriesHandler CreateSut() => new(_categoryRepository);

    [Fact]
    public async Task Handle_CategoriesExist_ReturnsMappedDtosWithCorrectType()
    {
        var category = Category.Create("Groceries", CategoryType.Expense);

        _categoryRepository
            .GetAllAsync(Arg.Any<CancellationToken>())
            .Returns([category]);

        var result = await CreateSut().Handle(new GetCategoriesQuery(), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Count.ShouldBe(1);
        result.Value[0].Id.ShouldBe(category.Id);
        result.Value[0].Name.ShouldBe("Groceries");
        result.Value[0].Type.ShouldBe(CategoryType.Expense);
    }
}
