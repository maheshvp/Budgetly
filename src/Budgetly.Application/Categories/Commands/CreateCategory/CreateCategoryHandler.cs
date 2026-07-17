using Budgetly.Application.Common.Interfaces;
using Budgetly.Application.Common.Results;
using Budgetly.Domain.Entities;
using MediatR;

namespace Budgetly.Application.Categories.Commands.CreateCategory;

public sealed class CreateCategoryHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var category = Category.Create(command.Name, command.Type);

        await categoryRepository.AddAsync(category, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(category.Id);
    }
}
