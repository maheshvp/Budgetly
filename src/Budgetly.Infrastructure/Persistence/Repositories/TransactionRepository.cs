using Budgetly.Application.Common.Interfaces;
using Budgetly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Budgetly.Infrastructure.Persistence.Repositories;

public sealed class TransactionRepository(AppDbContext dbContext) : ITransactionRepository
{
    public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Transactions.FindAsync([id], cancellationToken);

    public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default)
        => await dbContext.Transactions.AddAsync(transaction, cancellationToken);

    public async Task<IReadOnlyList<Transaction>> GetAllAsync(
        Guid? accountId,
        DateOnly? from,
        DateOnly? to,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Transaction> query = dbContext.Transactions;

        if (accountId.HasValue)
            query = query.Where(t => t.AccountId == accountId.Value);

        if (from.HasValue)
            query = query.Where(t => t.Date >= from.Value);

        if (to.HasValue)
            query = query.Where(t => t.Date <= to.Value);

        return await query.ToListAsync(cancellationToken);
    }
}
