using Budgetly.Application.Categories.Commands.CreateCategory;
using Budgetly.Application.Common.Interfaces;
using Budgetly.Domain.Entities;
using Budgetly.Domain.Enums;

namespace Budgetly.Application.Tests.Categories.Commands;

public sealed class CreateCategoryHandlerTests
{
    private readonly ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private CreateCategoryHandler CreateSut() => new(_categoryRepository, _unitOfWork);

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccessWithNewGuid()
    {
        var command = new CreateCategoryCommand("Salary", CategoryType.Income);

        var result = await CreateSut().Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBe(Guid.Empty);

        await _categoryRepository.Received(1).AddAsync(
            Arg.Is<Category>(c => c.Name == "Salary" && c.Type == CategoryType.Income),
            Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
