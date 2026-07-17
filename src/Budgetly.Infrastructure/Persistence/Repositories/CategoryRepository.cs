using Budgetly.Application.Common.Interfaces;
using Budgetly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Budgetly.Infrastructure.Persistence.Repositories;

public sealed class CategoryRepository(AppDbContext dbContext) : ICategoryRepository
{
    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Categories.FindAsync([id], cancellationToken);

    public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
        => await dbContext.Categories.AddAsync(category, cancellationToken);

    public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        => await dbContext.Categories.ToListAsync(cancellationToken);
}
