using Budgetly.Application.Common.Interfaces;
using Budgetly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Budgetly.Infrastructure.Persistence.Repositories;

public sealed class AccountRepository(AppDbContext dbContext) : IAccountRepository
{
    public async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Accounts.FindAsync([id], cancellationToken);

    public async Task AddAsync(Account account, CancellationToken cancellationToken = default)
        => await dbContext.Accounts.AddAsync(account, cancellationToken);

    public async Task<IReadOnlyList<Account>> GetAllAsync(CancellationToken cancellationToken = default)
        => await dbContext.Accounts.ToListAsync(cancellationToken);
}
